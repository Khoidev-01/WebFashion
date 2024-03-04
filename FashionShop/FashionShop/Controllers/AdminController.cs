using Microsoft.AspNetCore.Mvc;

namespace FashionShop.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
