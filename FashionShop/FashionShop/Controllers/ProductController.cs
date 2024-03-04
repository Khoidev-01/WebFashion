using FashionShop.Data;
using FashionShop.Models;
using FashionShop.Models.DTO.CommentDTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FashionShop.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductRepository _productRepository;
        public readonly ICategoryRepository _categoryRepository;
        public readonly FashionShopDBContext _context;
        public readonly ICommentRepository _commentRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, FashionShopDBContext context, ICommentRepository commentRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(int idCategory, int? idOrderProduct = null)
        {
            var listProductByCategory = await _productRepository.GetByCategoryId(idCategory, idOrderProduct);

            ViewBag.NameCategory = _categoryRepository.GetCategoryById(idCategory);
            ViewBag.IdCategory = idCategory;
            ViewBag.IdOrderProduct = idOrderProduct;

            return View(listProductByCategory);
        }

        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetId(id);
            @ViewBag.listImg = JsonConvert.DeserializeObject<List<string>>(product.ListImages);
            var lsprodcut = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CategoryID == product.CategoryID && x.ID != product.ID)
                    .ToList();

            @ViewBag.SanPham = lsprodcut;

            // Comment
            ViewBag.ListComment = _commentRepository.GetListComment(0, id);
            ViewBag.SumComment = _commentRepository.GetCountAll(id);

            return View(product);
        }

        [HttpPost]
        public JsonResult AddNewComment(CreateCommentDTO createCommentDTO)
        {
            try
            {
                var commentNew = _commentRepository.Create(createCommentDTO);

                if (commentNew != null)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            catch
            {
                return Json(new { status = false });
            }
        }

        [HttpPost]
        public JsonResult GetQuantityProductById(int id)
        {
            var quantity = _productRepository.GetQuantityById(id);
             
            return Json(new
            {
                data = quantity,
                status = true,
            });
        }
    }
}
