using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeRoles("Quản trị viên", "Nhân viên")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _icontactRepository;
        public ContactsController(IContactRepository icontactRepository)
        {
            _icontactRepository = icontactRepository;
        }

        [HttpGet("get-all-contact")]
        public IActionResult GetAll(int page = 0, int pageSize = 6, string? searchByPhoneNumber = null)
        {
            var allcontacts = _icontactRepository.GetAllContact(page, pageSize, searchByPhoneNumber);
            return Ok(allcontacts);
        }

        [HttpPut("cofirm-contact/{id}")]
        public IActionResult CofirmContact(int id)
        {
            var check = _icontactRepository.Cofirm(id);
            if(check == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        //[HttpPut("update-contact-by-id/{id}")]
        //public IActionResult UpdateContactById(int id, [FromBody] UpdateContactDTO updatecontactDTO)
        //{
        //    var updatContact = _icontactRepository.UpdateContactById(id, updatecontactDTO);
        //    return Ok(updatContact);
        //}

        //[HttpPost("add-contact")]
        //public IActionResult AddContact([FromBody] AddContactDTO addContactDTO)
        //{
        //    var addcontact = _icontactRepository.AddContact(addContactDTO);
        //    return Ok(addcontact);
        //}

        //[HttpDelete("delete-category-by-id/{id}")]
        //public IActionResult DeleteContact(int id)
        //{
        //    var deletecontact = _icontactRepository.DeleteContact(id);
        //    return Ok(deletecontact);
        //}
    }
}