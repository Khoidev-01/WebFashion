namespace FashionShop.Models.DTO
{
    public class PostDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }

    public class AddPostRequestDTO
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}
