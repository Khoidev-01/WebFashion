using FashionShop.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionShop.Models.DTO.FavoriteProductDTO
{
    public class CreateFavoriteProductDTO
    {
        public string UserID { get; set; }
        public int ProductID { get; set; }
    }
}
