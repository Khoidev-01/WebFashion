using System.ComponentModel.DataAnnotations;

namespace FashionShop.Models.DTO.UserDTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không đúng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
