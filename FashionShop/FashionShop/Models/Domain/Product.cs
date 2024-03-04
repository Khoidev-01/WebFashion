using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FashionShop.Models.Domain
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }  

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Describe { get; set; }

        [Required]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string? ListImages { get; set; }

        [Required]
        public double Price { get; set; }
        public double PurchasePrice { get; set; }

        public double Discount { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public bool Status { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<FavoriteProduct> FavoriteProducts { get; set; }

    }
}
