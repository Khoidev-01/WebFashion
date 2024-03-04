using System.Diagnostics.Eventing.Reader;

namespace FashionShop.Models.DTO.UserDTO
{
    public class GetUserDTO
    {
        public string ID { get; set; }
        public string FullName { get; set; }    
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
