using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO.ProductDTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get-list-products")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetListProducts(int page = 0, int pageSize = 6, int? searchByCategory = null, string? searchByName = null)
        {
            try
            {
                var listProducts = await _productRepository.GetAll(page, pageSize, searchByCategory, searchByName);

                return Ok(listProducts);    
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("get-product-by-id/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productRepository.GetById(id);

                if(product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return BadRequest("Không tìm thấy id của product");
                }

            }
            catch
            {
                return BadRequest("Lấy product theo id không thành công");
            }
        }

        [HttpPost("create-product")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO createProductDTO)
        {
            try
            {
                var product = await _productRepository.Create(createProductDTO);

                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return BadRequest("Tạo product không thành công");
                }

            }
            catch
            {
                return BadRequest("Tạo product không thành công");
            }
        }

        [HttpPut("update-product/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO, int id)
        {
            try
            {
                var product = await _productRepository.Update(updateProductDTO, id);
                if(product != null )
                {
                    return Ok(product);
                }
                else
                {
                    return BadRequest("Không tìm thấy id của sản phẩm");
                }
            }
            catch
            {
                return BadRequest("Chỉnh sửa sản phẩm không thành công");
            }
        }

        [HttpDelete("delete-product/{id}")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> DeleleProduct(int id)
        {
            try
            {
                var product = await _productRepository.Delete(id);
                if (product == true)
                {
                    return Ok("Xóa sản phẩm thành công");
                }
                else
                {
                    return BadRequest("Không tìm thấy id của sản phẩm");
                }
            }
            catch
            {
                return BadRequest("Xóa sản phẩm không thành công");
            }
        }

        [HttpGet("get-count-product")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetCountProduct()
        {
            try
            {
                var count = await _productRepository.Count();

                return Ok(count);
            }
            catch
            {
                return BadRequest("Lỗi");
            }
        }
    }
}

