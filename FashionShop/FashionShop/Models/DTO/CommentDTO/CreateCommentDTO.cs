namespace FashionShop.Models.DTO.CommentDTO
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public int ParentID { get; set; }
        public string UserID { get; set; }
        public int ProductID { get; set; }
    }
}
