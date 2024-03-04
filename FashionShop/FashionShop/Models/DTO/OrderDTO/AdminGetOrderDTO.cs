using FashionShop.Models.Domain;

namespace FashionShop.Models.DTO.OrderDTO
{
    public class AdminGetOrderDTO
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double DeliveryFee { get; set; }
        public DateTime OrderDate { get; set; }
        public int TypePayment { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public Voucher? Voucher { get; set; }
        public int Status { get; set; }
        public double TotalPayment { get; set; }
    }
}
