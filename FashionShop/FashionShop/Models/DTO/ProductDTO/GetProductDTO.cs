using FashionShop.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FashionShop.Models.DTO.ProductDTO
{
    public class GetProductDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public string Describe { get; set; }
        public string Image { get; set; }
        public string? ListImages { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool Status { get; set; }
    }
}
