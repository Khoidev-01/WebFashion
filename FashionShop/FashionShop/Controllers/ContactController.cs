using AspNetCoreHero.ToastNotification.Abstractions;
using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly FashionShopDBContext _context;
        public INotyfService _notifyServic { get; }
        public ContactController(FashionShopDBContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyServic = notifyService;
        }
        [HttpGet]
        ////[AllowAnonymous]
        //[Route("lien-he.html", Name = "LienHe")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(Contact contact)
        {
            try
            {
              
                if (ModelState.IsValid)
                {
                    Contact contact1 = new Contact
                    {
                        FullName = contact.FullName,
                        Email = contact.Email,
                        PhoneNumber = contact.PhoneNumber,
                        Content = contact.Content,
                        Status = false

                    };
                    try
                    {
                        _context.Add(contact1);
                        await _context.SaveChangesAsync();
                        _notifyServic.Success("Gửi thành công");
                        return View(contact);
                    }
                    catch
                    {

                        return RedirectToAction("Index");
                    }
                    
                }

                else
                {

                    //_notifyServic.Error("Gửi không thành công");
                    return View(contact);
                }
            }
            catch
            {

                return View(contact);
            }
            //return View(contact);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult checkPhone(string Phone)
        {
            try
            {
                var khachhang = _context.Contacts.AsNoTracking().SingleOrDefault(x => x.PhoneNumber.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }
    }
}
