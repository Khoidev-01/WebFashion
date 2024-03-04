using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO.CommentDTO;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Repositories
{
    public interface ICommentRepository
    {
        public List<GetCommentDTO> GetListComment(int parentID, int productID);
        public int GetCountAll(int productID);
        public CreateCommentDTO Create(CreateCommentDTO createCommentDTO);
    }
    public class CommentRepository : ICommentRepository
    {
        private readonly FashionShopDBContext _fashionShopDbContext;
        public CommentRepository(FashionShopDBContext fashionShopDbContext)
        {
            _fashionShopDbContext = fashionShopDbContext;
        }

        public List<GetCommentDTO> GetListComment(int parentID, int productID)
        {
            var getCommnent = _fashionShopDbContext.Comments.Select(comment => new GetCommentDTO()
            {
                ID = comment.ID,
                Content = comment.Content,
                CreatedDate = comment.CreatedDate,
                UpdatedDate = comment.UpdatedDate,
                ProductID = comment.ProductID,
                UserID = comment.UserID,
                FullName = comment.User.FullName,
                ParentID = comment.ParentID,
            }).OrderByDescending(c => c.ID).Where(c => c.ParentID == parentID && c.ProductID == productID).ToList();

            return getCommnent;
        }

        public int GetCountAll(int productID)
        {
            var countComment = _fashionShopDbContext.Comments.Where(c => c.ProductID == productID).Count();
            return countComment;
        }

        public CreateCommentDTO Create(CreateCommentDTO createCommentDTO)
        {
            var commentDomain = new Comment()
            {
                Content = createCommentDTO.Content,
                CreatedDate = DateTime.Now,
                ParentID = createCommentDTO.ParentID,
                UserID = createCommentDTO.UserID,
                ProductID = createCommentDTO.ProductID
            };

            _fashionShopDbContext.Comments.Add(commentDomain);
            _fashionShopDbContext.SaveChanges();

            return createCommentDTO;
        }
    }
}

