namespace FashionShop.Models.DTO.CommentDTO
{
    public class GetCommentDTO
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int ParentID { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public int ProductID { get; set; }
    }
}
