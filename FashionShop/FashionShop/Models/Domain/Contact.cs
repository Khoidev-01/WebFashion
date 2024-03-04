using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.Domain
{
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage ="Chiều dài họ tên phải lớn hơn 3")]
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        [Display(Name = "FullName")]
        
        public string FullName { get; set; }

        [MaxLength(150)]
        [Display(Name = "Địa chỉ email")]
        [EmailAddress(ErrorMessage ="Sai định dạng email")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage ="vui lòng nhập số điện thoại")]
        [Display(Name ="dien thoai")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action:"checkPhone", controller: "Contact")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}
