using FashionShop.Data;
using FashionShop.Helper;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO.ProductDTO;
using FashionShop.Models.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public interface IProductRepository
{
    public Task<AdminPaginationSet<GetProductDTO>> GetAll(int page, int pageSize, int? searchByCategory, string? searchByName);

    public List<GetProductDTO> GetAll(string? keyword = null);
    public List<SearchProductByNameDTO> GetAllByName(string keyword);
    public Task<GetProductByIdDTO> GetById(int idProduct);
    public Task<List<GetProductByIdDTO>> GetByCategoryId(int idCategory, int? idOrderProduct = null);
    public GetProductByIdDTO GetId(int id);
    public int GetQuantityById(int id);
    public Task<CreateProductDTO> Create(CreateProductDTO createProductDTO);
    public Task<UpdateProductDTO> Update(UpdateProductDTO updateProductDTO, int id);
    public Task<bool> Delete(int id);
    public Task<bool> ReduceQuantityOrder(List<ShoppingCartViewModel> listOrder);
    public Task<bool> IncreaseQuantityOrder(List<ShoppingCartViewModel> listOrder);
    public Task<int> Count();
}

public class ProductRepository : IProductRepository
{
    private readonly FashionShopDBContext _fashionShopDBContext;

    public ProductRepository(FashionShopDBContext fashionShopDBContext)
    {
        _fashionShopDBContext = fashionShopDBContext;
    }

    public async Task<AdminPaginationSet<GetProductDTO>> GetAll(int page, int pageSize, int? searchByCategory, string? searchByName)
    {
        var listProductDomain = _fashionShopDBContext.Products.AsQueryable();

        if(searchByCategory != null)
        {
            listProductDomain = listProductDomain.Where(p => p.CategoryID == searchByCategory);
        }

        if(searchByName != null)
        {
            listProductDomain = listProductDomain.Where(p => p.Name.Contains(searchByName));
        }

        var listProductDTO = await listProductDomain.Select(product => new GetProductDTO
        {
            ID = product.ID,
            Name = product.Name,
            CategoryName = product.Category.Name,
            Quantity = product.Quantity,
            Describe = product.Describe,
            Image = product.Image,
            ListImages = product.ListImages,
            Price = product.Price,
            Discount = product.Discount,
            CreatedDate = product.CreatedDate,
            CreatedBy = product.CreatedBy,
            UpdatedDate = product.UpdatedDate,
            UpdatedBy = product.UpdatedBy,
            Status = product.Status,
        }).OrderByDescending(p => p.ID).ToListAsync();

        var totalCount = listProductDTO.Count();
        var listProductPagination = listProductDTO.Skip(page * pageSize).Take(pageSize);

        AdminPaginationSet<GetProductDTO> productPaginationSet = new AdminPaginationSet<GetProductDTO>()
        {
            List = listProductPagination,
            Page = page,
            TotalCount = totalCount,
            PagesCount = (int)Math.Ceiling((decimal)totalCount / pageSize),
        };

        return productPaginationSet;
    }
    public  List<GetProductDTO> GetAll(string? keyword)
    {
        if(!string.IsNullOrEmpty(keyword))
        {
            var listProductDTO = _fashionShopDBContext.Products.Select(pro => new GetProductDTO
            {
                ID = pro.ID,
                Name = pro.Name,
                Describe = pro.Describe,
                Image = pro.Image,
                Price = pro.Price,
                Discount = pro.Discount,
                CreatedDate = pro.CreatedDate,
                Status = pro.Status,
            }).OrderByDescending(p => p.CreatedDate).Where(p => p.Status == true && p.Name.Contains(keyword)).ToList();
            return listProductDTO;
        }
        else
        {
            var listProductDTO = _fashionShopDBContext.Products.Select(pro => new GetProductDTO
            {
                ID = pro.ID,
                Name = pro.Name,
                Describe = pro.Describe,
                Image = pro.Image,
                Price = pro.Price,
                Discount = pro.Discount,
                CreatedDate = pro.CreatedDate,
                Status = pro.Status,
            }).OrderByDescending(p => p.CreatedDate).Where(p => p.Status == true).ToList();
            return listProductDTO;
        }
        
    }
    public List<SearchProductByNameDTO> GetAllByName(string keyword)
    {
        var listProductDomain = _fashionShopDBContext.Products
            .Where(product => product.Status == true && product.Name
            .Contains(keyword))
            .Select(product => new SearchProductByNameDTO()
        {
            ID = product.ID,
            Name = product.Name,
            Image = product.Image,
            Price = product.Price,
            Discount = product.Discount,
            CreatedDate = product.CreatedDate,
            Status = product.Status
        }).OrderByDescending(p => p.CreatedDate).ToList();

        return listProductDomain;
    }
    public async Task<GetProductByIdDTO> GetById(int idProduct)
    {
        var productDomain = await _fashionShopDBContext.Products.Select(product => new GetProductByIdDTO
        {
            ID = product.ID,
            Name = product.Name,
            CategoryID = product.CategoryID,
            CategoryName = product.Category.Name,
            Quantity = product.Quantity,
            Describe = product.Describe,
            Image = product.Image,
            ListImages = product.ListImages,
            Price = product.Price,
            PurchasePrice = product.PurchasePrice,
            Discount = product.Discount,
            CreatedDate = product.CreatedDate,
            CreatedBy = product.CreatedBy,
            UpdatedDate = product.UpdatedDate,
            UpdatedBy = product.UpdatedBy,
            Status = product.Status,
        }).FirstOrDefaultAsync(p => p.ID == idProduct);

        return productDomain;
    }

    public async Task<List<GetProductByIdDTO>> GetByCategoryId(int idCategory, int? idOrderProduct)
    {
        var listProductDomain = _fashionShopDBContext.Products.AsQueryable();

        if (idOrderProduct != null)
        {
            switch(idOrderProduct)
            {
                case 1:
                    {
                        listProductDomain = listProductDomain.OrderBy(p => p.Price);
                        break;
                    }
                case 2:
                    {
                        listProductDomain = listProductDomain.OrderByDescending(p => p.Price);
                        break;
                    }
                case 3:
                    {
                        listProductDomain = listProductDomain.OrderBy(p => p.Name);
                        break;
                    }
                case 4:
                    {
                        listProductDomain = listProductDomain.OrderByDescending(p => p.Name);
                        break;
                    }
                case 5:
                    {
                        listProductDomain = listProductDomain.OrderByDescending(p => p.CreatedDate);
                        break;
                    }
                case 6:
                    {
                        listProductDomain = listProductDomain.OrderBy(p => p.CreatedDate);
                        break;
                    }
            }
        }


        var listProductDTO = await listProductDomain.Select(product => new GetProductByIdDTO
        {
            ID = product.ID,
            Name = product.Name,
            CategoryID = product.CategoryID,
            CategoryName = product.Category.Name,
            Quantity = product.Quantity,
            Describe = product.Describe,
            Image = product.Image,
            ListImages = product.ListImages,
            Price = product.Price,
            Discount = product.Discount,
            CreatedDate = product.CreatedDate,
            CreatedBy = product.CreatedBy,
            UpdatedDate = product.UpdatedDate,
            UpdatedBy = product.UpdatedBy,
            Status = product.Status,
        }).Where(p => p.Status == true && p.CategoryID == idCategory).ToListAsync();

        return listProductDTO;
    }
    public  GetProductByIdDTO GetId(int id) 
    {
        var productDomain = _fashionShopDBContext.Products.Select(product => new GetProductByIdDTO
        {
            ID = product.ID,
            Name = product.Name,
            CategoryID = product.CategoryID,
            CategoryName = product.Category.Name,
            Quantity = product.Quantity,
            Describe = product.Describe,
            Image = product.Image,
            ListImages = product.ListImages,
            Price = product.Price,
            Discount = product.Discount,
            CreatedDate = product.CreatedDate,
            CreatedBy = product.CreatedBy,
            UpdatedDate = product.UpdatedDate,
            UpdatedBy = product.UpdatedBy,
            Status = product.Status,
        }).FirstOrDefault(p => p.ID == id);

        return productDomain;
    }

    public int GetQuantityById(int id)
    {
        var quantity = _fashionShopDBContext.Products.SingleOrDefault(p => p.ID == id).Quantity;

        return quantity;
    }

    public async Task<CreateProductDTO> Create(CreateProductDTO createProductDTO)
    {
        var productDomain = new Product
        {
            Name = createProductDTO.Name,
            CategoryID = createProductDTO.CategoryID,
            Quantity = createProductDTO.Quantity,
            Describe = createProductDTO.Describe,
            Image = createProductDTO.Image,
            ListImages = createProductDTO.ListImages,
            Price = createProductDTO.Price,
            PurchasePrice = createProductDTO.PurchasePrice,
            Discount = createProductDTO.Discount,
            Status = createProductDTO.Status,

            CreatedDate = DateTime.Now,
            CreatedBy = createProductDTO.CreatedBy,
        };
        await _fashionShopDBContext.Products.AddAsync(productDomain);
        await _fashionShopDBContext.SaveChangesAsync();

        return createProductDTO;
    }

    public async Task<UpdateProductDTO> Update(UpdateProductDTO updateProductDTO, int id)
    {
        var productDomain = await _fashionShopDBContext.Products.FirstOrDefaultAsync(p => p.ID == id);

        if(productDomain != null)
        {
            productDomain.Name = updateProductDTO.Name;
            productDomain.CategoryID = updateProductDTO.CategoryID;
            productDomain.Quantity = updateProductDTO.Quantity;
            productDomain.Describe = updateProductDTO.Describe;
            productDomain.Image = updateProductDTO.Image;
            productDomain.ListImages = updateProductDTO.ListImages;
            productDomain.Price = updateProductDTO.Price;
            productDomain.PurchasePrice = updateProductDTO.PurchasePrice;
            productDomain.Discount = updateProductDTO.Discount;
            productDomain.Status = updateProductDTO.Status;

            productDomain.UpdatedDate = DateTime.Now;
            productDomain.UpdatedBy = updateProductDTO.UpdatedBy;

            await _fashionShopDBContext.SaveChangesAsync();
            return updateProductDTO;
        }
        else
        {
            return null!;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var productDomain = await _fashionShopDBContext.Products.FirstOrDefaultAsync(p => p.ID == id);

        if(productDomain != null)
        {
            _fashionShopDBContext.Remove(productDomain);
            await _fashionShopDBContext.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> ReduceQuantityOrder(List<ShoppingCartViewModel> listOrder)
    {
        foreach(var item in listOrder)
        {
            var productById = await _fashionShopDBContext.Products.SingleOrDefaultAsync(p => p.ID == item.ProductID);

            if(productById != null)
            {
                productById.Quantity -= item.Quantity;
                await _fashionShopDBContext.SaveChangesAsync();
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> IncreaseQuantityOrder(List<ShoppingCartViewModel> listOrder)
    {
        foreach (var item in listOrder)
        {
            var productById = await _fashionShopDBContext.Products.SingleOrDefaultAsync(p => p.ID == item.ProductID);

            if (productById != null)
            {
                productById.Quantity += item.Quantity;
                await _fashionShopDBContext.SaveChangesAsync();
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public async Task<int> Count()
    {
        var countProduct = await _fashionShopDBContext.Products.CountAsync();
        return countProduct;
    }
}

