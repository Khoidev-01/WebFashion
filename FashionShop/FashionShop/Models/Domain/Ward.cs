using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.Domain
{
    [Table("Wards")]
    public class Ward
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int ProvinceID { get; set; }
        public Province Province { get; set; }

        public int DistrictID { get; set; }
        public District District { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
