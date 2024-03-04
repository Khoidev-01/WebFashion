using System.ComponentModel.DataAnnotations;

namespace FashionShop.Models.DTO.RolesDTO
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
    }
}
