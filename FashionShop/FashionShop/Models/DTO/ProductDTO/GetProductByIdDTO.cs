namespace FashionShop.Models.DTO.ProductDTO
{
    public class GetProductByIdDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public string Describe { get; set; }
        public string Image { get; set; }
        public string? ListImages { get; set; }
        public double Price { get; set; }
        public double PurchasePrice { get; set; }
        public double Discount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool Status { get; set; }
    }
}
