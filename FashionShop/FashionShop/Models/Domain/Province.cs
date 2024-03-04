using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.Domain
{
    [Table("Provinces")]
    public class Province
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<District> Districts { get; set; }
        public IEnumerable<Ward> Wards { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
