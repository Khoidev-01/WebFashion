using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FashionShop.Repositories
{
    public interface ICategoryRepository
    {
        List<CategoryDTO> GetAllCategory();
        CategoryByIdDTO GetCategoryById(int id);
        Category AddCategory(AddCategoryRequestDTO addCategoryRequestDTO);
        AddCategoryRequestDTO? UpdateCategoryById(int id, AddCategoryRequestDTO CategoryDTO);
        Category DeleteCategoryById(int id);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FashionShopDBContext _IdentityDbContext;
        public CategoryRepository(FashionShopDBContext IdentityDbContext)
        {
            _IdentityDbContext = IdentityDbContext;
        }
        public List<CategoryDTO> GetAllCategory()
        {
            var allCategory = _IdentityDbContext.Categories.Select(Categorys => new CategoryDTO()
            {
                ID = Categorys.ID,
                Name = Categorys.Name
            }).ToList();
            return allCategory;
        }
        public CategoryByIdDTO GetCategoryById(int id)
        {
            var CategoryWithDomain = _IdentityDbContext.Categories.Where(n => n.ID == id);
            var CategoryWithIdDTO = CategoryWithDomain.Select(Categorys => new CategoryByIdDTO()
            {
                Name = Categorys.Name
            }).FirstOrDefault();
            return CategoryWithIdDTO;
        }
        public Category AddCategory(AddCategoryRequestDTO addCategoryRequestDTO)
        {
            var CategoryDomainModel = new Category
            {
                Name = addCategoryRequestDTO.Name
            };
            _IdentityDbContext.Categories.Add(CategoryDomainModel);
            _IdentityDbContext.SaveChanges();
            return CategoryDomainModel;
        }
        public AddCategoryRequestDTO? UpdateCategoryById(int id, AddCategoryRequestDTO CategoryDTO)
        {
            var CategoryDomain = _IdentityDbContext.Categories.FirstOrDefault(n => n.ID == id);
            if (CategoryDomain != null)
            {
                CategoryDomain.Name = CategoryDTO.Name;
                _IdentityDbContext.SaveChanges();
            }
            return CategoryDTO;
        }
        public Category DeleteCategoryById(int id)
        {
            var CategoryDomain = _IdentityDbContext.Categories.FirstOrDefault(n => n.ID == id);
            if(CategoryDomain != null)
            {
                _IdentityDbContext.Categories.Remove(CategoryDomain);
                _IdentityDbContext.SaveChanges();
            }
            return CategoryDomain;
        }
    }
}
