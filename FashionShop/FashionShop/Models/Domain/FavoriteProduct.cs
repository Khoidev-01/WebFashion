using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.Domain
{
    [Table("FavoriteProducts")]
    public class FavoriteProduct
    {
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
