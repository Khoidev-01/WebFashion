using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FashionShop.Models.Domain
{
    [Table("Vouchers")]
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string DiscountCode { get; set; }

        [Required]
        public bool DiscountAmount { get; set; }

        [Required]
        public bool DiscountPercentage { get; set; }

        [Required]
        public double DiscountValue { get; set; }

        public double MinimumValue { get; set;}

        [Required]
        public int Quantity { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Describe { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
