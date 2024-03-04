using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.Domain
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int ID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        
        public int ProvinceID { get; set; }
        public Province Province { get; set; }

        public int DistrictID { get; set; }
        [ForeignKey("DistrictID")]
        public District District { get; set; }


        public int WardID { get; set; }
        [ForeignKey("WardID")]
        public Ward Ward { get; set; }


        [Required]
        public string Address { get; set; }

        public string? Note { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }

        public double DeliveryFee { get; set; }

        public int Status { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public int? VoucherID { get; set; }
        [ForeignKey("VoucherID")]
        public Voucher? Voucher { get; set; }

        public int TypePayment { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
