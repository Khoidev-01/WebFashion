using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.DTO.VoucherDTO
{
    public class GetVoucherDTO
    {
        public int ID { get; set; }
        public string DiscountCode { get; set; }
        public bool DiscountAmount { get; set; }
        public bool DiscountPercentage { get; set; }
        public double DiscountValue { get; set; }
        public double MinimumValue { get; set; }
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Describe { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
