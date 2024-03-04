using FashionShop.Models.Domain;
using FashionShop.Models.DTO.UserDTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Blogger_Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using FashionShop.Helper;
using FashionShop.Models.ViewModel;
using FashionShop.Models.DTO.FavoriteProductDTO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace FashionShop.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IFavoriteProductRepository _favoriteProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotyfService _notyfService;


        public AccountController(UserManager<User> userManager, IUserRepository userRepository, IOrderRepository orderRepository, IFavoriteProductRepository favoriteProductRepository, IProductRepository productRepository, INotyfService notyfService)
        {
            _userManager = userManager;
            _userRepository = userRepository; 
            _orderRepository = orderRepository;
            _favoriteProductRepository = favoriteProductRepository;
            _productRepository = productRepository;
            _notyfService = notyfService;
        }

        public IActionResult Login()
        {
            // Lấy đường dẫn của trang trước đó (referrer)
            string referrerUrl = HttpContext.Request.Headers["Referer"].ToString();

            ViewBag.ReturnUrl = referrerUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (user != null && checkPasswordResult == true)
                {
                    if(user.LockoutEnabled == true)
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                    }
                    else
                    {
                        // Serialize đối tượng User thành JSON và lưu vào session
                        string userJson = JsonConvert.SerializeObject(user);
                        HttpContext.Session.SetString(CommonConstants.SessionUser, userJson);

                        _notyfService.Success("Đăng nhập thành công", 2);

                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CommonConstants.SessionUser);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var customerByEmail = await _userManager.FindByEmailAsync(registerRequestDTO.Email);
                if (customerByEmail != null)
                {
                    ModelState.AddModelError("Email", "Email này đã tồn tại");
                    return View();
                }

                if (registerRequestDTO.Password != registerRequestDTO.RePassword)
                {
                    ModelState.AddModelError("Password", "Mật khẩu không trùng khớp với nhau");
                    return View();
                }

                var CustomerRegister = await _userRepository.RegisterAccountCustomer(registerRequestDTO);

                if (CustomerRegister == true)
                {
                    ViewData["SuccessMsg"] = " Đăng ký thành công";

                    string template = System.IO.File.ReadAllText("assets/customer/template/register.html");
                    template = template.Replace("{{userName}}", registerRequestDTO.Email);
                    template = template.Replace("{{linkLogin}}", registerRequestDTO.Email);

                    EmailService.SendMail("FashionShop", "Đăng ký tài khoản thành công", template, registerRequestDTO.Email);
                }
            }
            return View();
        }
    
        public IActionResult Information()
        {
            var userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);

            var user = JsonConvert.DeserializeObject<User>(userSession);

            return View(user);
        }

        public async Task<IActionResult> OrderList()
        {
            var userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);
            var user = JsonConvert.DeserializeObject<User>(userSession);

            var order = await _orderRepository.GetByUserID(user.Id);

            return View(order);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var orderDetail = await _orderRepository.GetById(id);

            return View(orderDetail);
        }

        public async Task<JsonResult> OrderCancel(int id)
        {
            var orderCancel = await _orderRepository.Cancel(id);

            if(orderCancel != null)
            {
                // Tăng số lượng sản phẩm khi hủy hàng
                var increaseQuantityProduct = await _productRepository.IncreaseQuantityOrder(orderCancel);

                if (increaseQuantityProduct == true)
                {
                    _notyfService.Success("Đã hủy đơn hàng", 2);
                    return Json(new
                    {
                        status = true
                    });
                }
            }

            return Json(new
            {
                status = false
            });
        }

        public async Task<IActionResult> FavoriteList()
        {
            var userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);
            var user = JsonConvert.DeserializeObject<User>(userSession);

            var listFavoriteProduct = await _favoriteProductRepository.GetByUserID(user.Id);

            return View(listFavoriteProduct);
        }

        [HttpPost]
        public async Task<JsonResult> AddFavoriteProduct(CreateFavoriteProductDTO createFavoriteProductDTO)
        {
            var userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);

            if(userSession != null)
            {
                var user = JsonConvert.DeserializeObject<User>(userSession);

                createFavoriteProductDTO.UserID = user.Id;

                var favoriteProduct = await _favoriteProductRepository.Create(createFavoriteProductDTO);

                if (favoriteProduct != null)
                {
                    var product = await _productRepository.GetById(createFavoriteProductDTO.ProductID);

                    _notyfService.Custom("<img style='height: 40px; padding-right: 10px;' src='" + product.Image + "'/> Đã thêm vào yêu thích", 2, "white");

                    return Json(new
                    {
                        status = true
                    });
                }
            }

            _notyfService.Error("Vui lòng đăng nhập", 2);
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteFavoriteProduct(int productID)
        {
            var userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);

            if (userSession != null)
            {
                var user = JsonConvert.DeserializeObject<User>(userSession);

                var favoriteProduct = await _favoriteProductRepository.Delete(productID, user.Id);

                if (favoriteProduct != null)
                {
                    _notyfService.Success("Đã xóa khỏi yêu thích", 2);
                    return Json(new
                    {
                        status = true
                    });
                }
            }

            return Json(new
            {
                status = false
            });
        }
    } 
}
