using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.Domain
{
    [Table("Districts")]
    public class District
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int ProvinceID { get; set; }
        [ForeignKey("ProvinceID")]
        public Province Province { get; set; }

        public IEnumerable<Ward> Wards { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
