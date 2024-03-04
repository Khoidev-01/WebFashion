using FashionShop.Models.DTO;
using FashionShop.Models.Domain;
using FashionShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.IdentityModel.Tokens;
using FashionShop.Helper;
using FashionShop.Models.DTO.ProductDTO;

namespace FashionShop.Repositories
{
    public interface IContactRepository
    {
        AdminPaginationSet<ContactDTO> GetAllContact(int page, int pageSize, string? searchByPhoneNumber);
        bool Cofirm(int id);
        //AddContactDTO AddContact(AddContactDTO addContactDTO);
        //UpdateContactDTO UpdateContactById(int id, UpdateContactDTO updatecontactDTO);
        //Contact DeleteContact(int id);
    }
    public class ContactRepository : IContactRepository
    {
        private readonly FashionShopDBContext _identityDbContext;
        public ContactRepository(FashionShopDBContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        public AdminPaginationSet<ContactDTO> GetAllContact(int page, int pageSize, string? searchByPhoneNumber)
        {
            var allContact = _identityDbContext.Contacts.AsQueryable();

            if(!searchByPhoneNumber.IsNullOrEmpty())
            {
                allContact = allContact.Where(c => c.PhoneNumber.Contains(searchByPhoneNumber));    
            }

            var allContactDomain = allContact.Select(c => new ContactDTO
            {
                ID = c.ID,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Content = c.Content,
                Status = c.Status,

            }).OrderByDescending(c => c.ID).ToList();

            var totalCount = allContactDomain.Count();
            var listContactPagination = allContactDomain.Skip(page * pageSize).Take(pageSize);

            AdminPaginationSet<ContactDTO> contactPaginationSet = new AdminPaginationSet<ContactDTO>()
            {
                List = listContactPagination,
                Page = page,
                TotalCount = totalCount,
                PagesCount = (int)Math.Ceiling((decimal)totalCount / pageSize),
            };

            return contactPaginationSet;
        }
        
        public bool Cofirm(int id)
        {
            var cofirmContact = _identityDbContext.Contacts.FirstOrDefault(c => c.ID == id);
            
            if(cofirmContact != null)
            {
                cofirmContact.Status = true;

                _identityDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        //public AddContactDTO AddContact(AddContactDTO addContactDTO)
        //{
        //    var ContactDM = new Contact
        //    {
        //        FullName = addContactDTO.FullName,
        //        Email = addContactDTO.Email,
        //        PhoneNumber = addContactDTO.PhoneNumber,
        //        Content = addContactDTO.Content,
        //        Status = true,
        //    };
        //    _identityDbContext.Contacts.Add(ContactDM);
        //    _identityDbContext.SaveChanges();
        //    return addContactDTO;
        //}
        //public UpdateContactDTO UpdateContactById(int id, UpdateContactDTO updatecontactDTO)
        //{
        //    var ContactDm = _identityDbContext.Contacts.FirstOrDefault(n => n.ID == id);
        //    if (ContactDm != null)
        //    {
        //        ContactDm.FullName = updatecontactDTO.FullName;
        //        ContactDm.PhoneNumber = updatecontactDTO.PhoneNumber;
        //        ContactDm.Email = updatecontactDTO.Email;
        //        _identityDbContext.SaveChanges();
        //    }
        //    return updatecontactDTO;
        //}
        //public Contact DeleteContact(int id)
        //{
        //    var DeletedM = _identityDbContext.Contacts.FirstOrDefault(n => n.ID == id);
        //    if (DeletedM != null)
        //    {
        //        _identityDbContext.Contacts.Remove(DeletedM);
        //        _identityDbContext.SaveChanges();
        //    }
        //    return DeletedM;
        //}
    }
}