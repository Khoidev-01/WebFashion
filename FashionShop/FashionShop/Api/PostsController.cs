using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeRoles("Quản trị viên", "Nhân viên")]
    public class PostsController : ControllerBase
    {
        private readonly FashionShopDBContext _dbContext;
        public PostsController(FashionShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("get-all-post")]
        public IActionResult GetAll()
        {
            var allPostDomain = _dbContext.Posts;
            var allPostDTO = allPostDomain.Select(Post => new PostDTO()
            {
                ID = Post.ID,
                Title = Post.Title,
                Image = Post.Image,
                Content = Post.Content,
                Status = Post.Status,
            }).ToList();
            return Ok(allPostDTO);
        }

        [HttpGet]
        [Route("get-post-by-id/{id}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            var postwithDomain = _dbContext.Posts.Where(n => n.ID == id);
            if (postwithDomain == null)
            {
                return NotFound();
            }
            var postWithIDDTO = postwithDomain.Select(Post => new PostDTO()
            {
                ID = Post.ID,
                Title = Post.Title,
                Image = Post.Image,
                Content = Post.Content,
                Status = Post.Status,
            }).FirstOrDefault();
            return Ok(postWithIDDTO);
        }

        [HttpPost("add-post")]
        public IActionResult AddPost([FromBody] AddPostRequestDTO addPostRequestDTO)
        {
            var postDomainModel = new Post
            {
                Title = addPostRequestDTO.Title,
                Image = addPostRequestDTO.Image,
                Content = addPostRequestDTO.Content,
                Status = addPostRequestDTO.Status,
            };
            _dbContext.Posts.Add(postDomainModel);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPut("update-post-by-id/{id}")]
        public IActionResult UpdatePostById(int id, [FromBody] AddPostRequestDTO postDTO)
        {
            var postDomain = _dbContext.Posts.FirstOrDefault(n => n.ID == id);
            if (postDomain != null)
            {
                postDomain.Title = postDTO.Title;
                postDomain.Image = postDTO.Image;
                postDomain.Content = postDTO.Content;
                postDomain.Status = postDTO.Status;
                _dbContext.SaveChanges();

                return Ok(postDTO);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete-post-by-id/{id}")]
        public IActionResult DeletePostByID(int id)
        {
            var postDomain = _dbContext.Posts.FirstOrDefault(n => n.ID == id);
            if (postDomain != null)
            {
                _dbContext.Posts.Remove(postDomain);
                _dbContext.SaveChanges();
                return Ok("Xóa bài viết thành công");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
