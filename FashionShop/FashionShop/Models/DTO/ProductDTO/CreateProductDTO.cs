using FashionShop.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FashionShop.Models.DTO.ProductDTO
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Chưa chọn danh mục sản phẩm")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Vui lòng điền số lượng sản phẩm")]
        [Range(1, float.MaxValue, ErrorMessage = "Số lượng phải nhiều hơn 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Describe { get; set; }


        [Required(ErrorMessage = "Vui lòng chọn hình ảnh")]
        public string Image { get; set; }

        public string? ListImages { get; set; }

        [Required(ErrorMessage = "Vui lòng điền giá bán")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá bán phải nhiều hơn 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Vui lòng điền giá nhập")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá nhập phải nhiều hơn 0")]
        public double PurchasePrice { get; set; }

        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm từ 0 đến 100")]
        public double Discount { get; set; }
        public string CreatedBy { get; set; }
        public bool Status { get; set; }
    }
}
