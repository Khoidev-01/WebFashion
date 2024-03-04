using FashionShop.Models.Domain;
using FashionShop.Models.DTO.ProductDTO;

namespace FashionShop.Models.ViewModel
{
    //[Serializable]
    public class ShoppingCartViewModel
    {
        public int ProductID { get; set; }
        public GetProductByIdDTO Product { get; set; }
        public int Quantity { get; set; }
    }
}
