using FashionShop.Api;
using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Controllers
{
    public class PostController : Controller
    {
        private readonly FashionShopDBContext _dbContext;
        public PostController(FashionShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var allPostDTO = _dbContext.Posts
            .Select(Post => new PostDTO()
            {
                ID = Post.ID,
                Title = Post.Title,
                Image = Post.Image,
                Content = Post.Content,
                Status = Post.Status,
            })
            .ToList();

            return View(allPostDTO);
        }
        public IActionResult Detail(int id)
        {
            // Lấy thông tin bài đăng dựa vào id từ cơ sở dữ liệu
            var post = _dbContext.Posts.FirstOrDefault(p => p.ID == id);

            if (post == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy bài đăng
            }

            var postDTO = new PostDTO()
            {
                ID = post.ID,
                Title = post.Title,
                Image = post.Image,
                Content = post.Content,
                Status = post.Status,
            };

            return View(postDTO); // Truyền dữ liệu vào view chi tiết
        }
    }
}
