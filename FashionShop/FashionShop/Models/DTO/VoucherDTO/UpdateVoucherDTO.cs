using System.ComponentModel.DataAnnotations;

namespace FashionShop.Models.DTO.VoucherDTO
{
    public class UpdateVoucherDTO
    {

        [Required(ErrorMessage = "Mã giảm giá không được để trống")]
        public string DiscountCode { get; set; }
        public bool DiscountAmount { get; set; }
        public bool DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá trị giảm")]
        public double DiscountValue { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá trị đơn hàng tối thiếu có thể áp dụng mã giảm giá")]
        public double MinimumValue { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng mã giảm giá")]
        public int Quantity { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Describe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái của mã giảm giá")]
        public bool Status { get; set; }

        public string UpdatedBy { get; set; }
    }
}
