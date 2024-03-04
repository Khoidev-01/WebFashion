using FashionShop.Models.Domain;
using FashionShop.Models.DTO.ProductDTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.DTO.FavoriteProductDTO
{
    public class GetFavoriteProductByUserIdDTO
    {
        public string UserID { get; set; }
        public int ProductID { get; set; }
        public Product ProductDTO { get; set; }
        public DateTime AddedDate { get; set; }
    }
}

