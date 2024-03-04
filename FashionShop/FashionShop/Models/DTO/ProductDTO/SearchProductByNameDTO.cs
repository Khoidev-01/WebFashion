namespace FashionShop.Models.DTO.ProductDTO
{
    public class SearchProductByNameDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
