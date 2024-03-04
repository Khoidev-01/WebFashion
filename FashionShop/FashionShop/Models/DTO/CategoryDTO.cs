using FashionShop.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Models.DTO
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class CategoryByIdDTO
    {
        public string Name { get; set; }
    }
    public class AddCategoryRequestDTO
    {
        public string Name { get; set; }
    }
}
