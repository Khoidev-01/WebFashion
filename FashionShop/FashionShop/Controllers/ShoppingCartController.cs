using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Blogger_Common;
using FashionShop.Helper;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO.OrderDTO;
using FashionShop.Models.DTO.UserDTO;
using FashionShop.Models.ViewModel;
using FashionShop.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Xml.Linq;
using WebBanHangOnline.Models.Payments;

namespace FashionShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardRepository _wardRepository;
        private readonly INotyfService _notyfService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartController(IProductRepository productRepository, IVoucherRepository voucherRepository, IOrderRepository orderRepository, IProvinceRepository provinceRepository, IDistrictRepository districtRepository, IWardRepository wardRepository, INotyfService notyfService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _voucherRepository = voucherRepository;
            _orderRepository = orderRepository;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _notyfService = notyfService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            var cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);

            if (cartSession == null)
            {
                var emptyCart = new List<ShoppingCartViewModel>();

                HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(emptyCart));

                cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);
            };

            return View();
        }

        public JsonResult GetAll()
        {
            var cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);

            if (cartSession == null)
            {
                var emptyCart = new List<ShoppingCartViewModel>();

                HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(emptyCart));

                cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);
            };

            var cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartSession);

            return Json(new
            {
                data = cart,
                status = true,
            });
        }

        [HttpPost]
        public async Task<JsonResult> Add(int productID, int quantity)
        {
            var cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);

            if (cartSession == null)
            {
                var emptyCart = new List<ShoppingCartViewModel>();

                HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(emptyCart));
            };

            var cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartSession);

            if (cart.Any(c => c.ProductID == productID))
            {
                foreach (var item in cart)
                {
                    if (item.ProductID == productID)
                    {
                        item.Quantity += quantity;
                    }
                }
            }
            else
            {
                var newItem = new ShoppingCartViewModel();
                newItem.ProductID = productID;
                newItem.Quantity = quantity;
                newItem.Product = _productRepository.GetId(productID);

                cart.Add(newItem);
            }

            HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(cart));

            var product = await _productRepository.GetById(productID);
            _notyfService.Custom("<img style='height: 40px; padding-right: 10px;' src='/" + product.Image + "'/> Đã thêm vào giỏ hàng", 2, "white");

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartData);

            var cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);

            var cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartSession);

            foreach (var item in cart)
            {
                foreach (var item2 in cartViewModel)
                {
                    if (item.ProductID == item2.ProductID)
                    {
                        item.Quantity = item2.Quantity;
                    };
                };
            };

            HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(cart));


            return Json(new
            {
                status = true,
            });
        }


        [HttpPost]
        public JsonResult Delete(int productID)
        {
            var cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);

            if (cartSession != null)
            {
                var cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartSession);

                cart.RemoveAll(c => c.ProductID == productID);

                HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(cart));

                _notyfService.Success("Đã xóa khỏi giỏ hàng", 2);
                return Json(new
                {
                    status = true,
                });
            };

            return Json(new
            {
                status = false,
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            var emptyCart = new List<ShoppingCartViewModel>();

            HttpContext.Session.SetString(CommonConstants.SessionCart, JsonConvert.SerializeObject(emptyCart));

            return Json(new
            {
                status = true,
            });
        }

        public IActionResult CheckOut()
        {
            string userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);

            if (userSession == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(userSession);

                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CreateOrderDTO createOrderDTO)
        {
            if(string.IsNullOrEmpty(createOrderDTO.FullName))
            {
                ViewBag.errorFullName = "Vui lòng nhập tên";
            }

            if (string.IsNullOrEmpty(createOrderDTO.PhoneNumber))
            {
                ViewBag.errorPhoneNumber = "Vui lòng nhập số điện thoại";
            }

            if (string.IsNullOrEmpty(createOrderDTO.Address))
            {
                ViewBag.errorAddress = "Vui lòng nhập địa chỉ giao hàng";
            }
            
            if (createOrderDTO.ProvinceID == 0)
            {
                ViewBag.errorProvinceID = "Vui lòng chọn Tỉnh / Thành";
            }
            
            if (createOrderDTO.DistrictID == 0)
            {
                ViewBag.errorDistrictID = "Vui lòng chọn Quận / Huyện";
            }
            
            if (createOrderDTO.WardID == 0)
            {
                ViewBag.errorWardID = "Vui lòng chọn Phường / Xã";
            }

            var code = new
            {
                Success = false,
                Code = -1,
                Url = ""
            };

            if(ModelState.IsValid)
            {
                string cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);
                var cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartSession);

                createOrderDTO.shoppingCarts = cart;

                var order = await _orderRepository.Create(createOrderDTO);

                if (order != null)
                {
                    // Giảm số lượng mã giảm giá trong csdl khi đặt hàng (nếu có)
                    if (order.VoucherID != null)
                    {
                        var reduceQuantityVoucher = await _voucherRepository.ReduceQuantityVoucher((int)order.VoucherID);
                    }

                    // Giảm số lượng sản phẩm trong csdl khi đặt hàng
                    var reduceQuantityProduct = await _productRepository.ReduceQuantityOrder(order.shoppingCarts!);

                    if (reduceQuantityProduct == true) 
                    {
                        code = new
                        {
                            Success = true,
                            Code = order.TypePayment,
                            Url = ""
                        };

                        if (order.TypePayment == 2)
                        {
                            var url = await UrlPayment(order.TypePayment, order.ID);
                            code = new
                            {
                                Success = true,
                                Code = order.TypePayment,
                                Url = url
                            };
                        }                        
                    }

                }

                if (code.Code == 1)
                {
                    return RedirectToAction("OrderSuccess");

                }
                else
                {
                    return Redirect(code.Url);
                }
            }
        

            // Tự gán lại giá trị khi nhập sai
            string userSession = HttpContext.Session.GetString(CommonConstants.SessionUser);
            var user = JsonConvert.DeserializeObject<User>(userSession);

            return View(user);
        }

        public async Task<IActionResult> OrderSuccess()
        {
            HttpContext.Session.Remove(CommonConstants.SessionCart);

            var sessionUser = HttpContext.Session.GetString(CommonConstants.SessionUser);
            var user = JsonConvert.DeserializeObject<User>(sessionUser);

            var order = await _orderRepository.GetNewByUserID(user.Id);

            // Gửi đơn hàng đến email của khách hàng khi vừa đặt hàng thành công
            var html = "";
            double totalMoney = 0;

            foreach (var item in order.OrderDetails)
            {
                html += "<tr>";

                var productName = item.Product.Name;
                var quantity = item.Quantity;
                if (item.Product.Discount > 0)
                {
                    var price = item.Product.Price - (item.Product.Price * item.Product.Discount / 100);
                    totalMoney += price * quantity;

                    html += "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + productName + "</td>"
                    + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + price.ToString("C0", new CultureInfo("vi-VN")) + "</td>\r\n"
                    + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + quantity + "</td>\r\n"
                    + "<td style=\"border: 1px solid black; padding: 8px; text-align: right;\">" + (price * quantity).ToString("C0", new CultureInfo("vi-VN")) + "</td>";
                }
                else
                {
                    var price = item.Product.Price;
                    totalMoney += price * quantity;

                    html += "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + productName + "</td>"
                   + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + price.ToString("C0", new CultureInfo("vi-VN")) + "</td>\r\n"
                   + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + quantity + "</td>\r\n"
                   + "<td style=\"border: 1px solid black; padding: 8px; text-align: right;\">" + (price * quantity).ToString("C0", new CultureInfo("vi-VN")) + "</td>";
                }

                html += "</tr>";
            }

            double totalPayment = totalMoney + order.DeliveryFee;

            string template = System.IO.File.ReadAllText("assets/customer/template/order.html");

            template = template.Replace("{{orderShopping}}", html);
            template = template.Replace("{{totalMoney}}", totalMoney.ToString("C0", new CultureInfo("vi-VN")));
            template = template.Replace("{{deliveryFee}}", order.DeliveryFee.ToString("C0", new CultureInfo("vi-VN")));
            double voucherValue = 0;
            if (order.Voucher != null)
            {
                if (order.Voucher.DiscountAmount == true)
                {
                    voucherValue = order.Voucher.DiscountValue;
                }
                else
                {
                    voucherValue = totalMoney * order.Voucher.DiscountValue / 100;
                }

                template = template.Replace("{{voucher}}", voucherValue.ToString("C0", new CultureInfo("vi-VN")));
                totalPayment -= voucherValue;
            }
            else
            {
                template = template.Replace("{{voucher}}", "0");
            }
            template = template.Replace("{{totalPayment}}", totalPayment.ToString("C0", new CultureInfo("vi-VN")));

            template = template.Replace("{{fullName}}", order.FullName);
            template = template.Replace("{{email}}", order.Email);
            template = template.Replace("{{phoneNumber}}", order.PhoneNumber);

            template = template.Replace("{{province}}", order.ProvinceName.ToString());
            template = template.Replace("{{district}}", order.DistrictName.ToString());
            template = template.Replace("{{ward}}", order.WardName.ToString());
            template = template.Replace("{{address}}", order.Address);
            template = template.Replace("{{note}}", order.Note);

            EmailService.SendMail("FashionShop", "Đặt hàng thành công", template, user.Email);

            return View(order);
        }

        public JsonResult LoadProvince()
        {
            var listProvinces = _provinceRepository.GetAll();
            return Json(new
            {
                data = listProvinces.OrderBy(p => p.Name),
                status = true,
            });

        }

        public JsonResult LoadDistrict(int provinceID)
        {
            var listDistricts = _districtRepository.getAll(provinceID);

            return Json(new
            {
                data = listDistricts.OrderBy(p => p.Name),
                status = true,
            });

        }

        public JsonResult LoadWard(int provinceID, int districtID)
        {

            var listWards = _wardRepository.GetAll(provinceID, districtID);

            return Json(new
            {
                data = listWards.OrderBy(p => p.Name),
                status = true,
            });

        }

        public JsonResult LoadTotalMoneyOfCart()
        {
            var cartSession = HttpContext.Session.GetString(CommonConstants.SessionCart);
            var cart = new List<ShoppingCartViewModel>();

            if (cartSession != null)
            {
                cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartSession);
            }

            double totalMoney = 0;
            foreach (var item in cart)
            {
                if (item.Product.Discount > 0)
                {
                    var price = item.Product.Price - (item.Product.Price * item.Product.Discount / 100);
                    totalMoney += price * item.Quantity;
                }
                else
                {
                    totalMoney += item.Product.Price * item.Quantity;
                }
            }

            return Json(new
            {
                data = totalMoney,
                status = true,
            });

        }

        public async Task<JsonResult> CheckVoucher(string discountCode)
        {
            var voucher = await _voucherRepository.GetByDiscountCode(discountCode);

            if (voucher != null)
            {
                return Json(new
                {
                    data = voucher,
                    status = true,
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                });
            }
        }

        public async Task<IActionResult> VnpayReturn()
        {
            if (HttpContext.Request.Query.Count > 0)
            {
                string vnp_HashSecret = _configuration.GetSection("VnpConfig:vnp_HashSecret").Value; //Chuoi bi mat
                //var vnpayData = Request.QueryString;
                var vnpayData = Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();

                //foreach (var s in vnpayData)
                //{
                //    //get all querystring data
                //    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                //    {
                //        vnpay.AddResponseData(s, vnpayData[s]);
                //    }
                //}

                foreach (var kvp in vnpayData)
                {
                    string key = kvp.Key;
                    string value = kvp.Value;
                    // Tiếp tục xử lý dữ liệu tại đây

                    if (key.StartsWith("vnp_") && !string.IsNullOrEmpty(value))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }

                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = HttpContext.Request.Query["vnp_SecureHash"];
                String TerminalID = HttpContext.Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = HttpContext.Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        int idOrder = int.Parse(orderCode);
                        var itemOrder = await _orderRepository.GetById(idOrder);
                        if (itemOrder != null)
                        {
                            //itemOrder.Status = 3; //đã thanh toán
                            //db.Orders.Attach(itemOrder);
                            //db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();

                            //Thanh toan thanh cong
                            return RedirectToAction("OrderSuccess");
                        }
                    }
                    else
                    {
                        _notyfService.Error("Thanh toán không thành công", 3);
                        return RedirectToAction("CheckOut");
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            return View();
        }

        #region Thanh toán vnpay
        public async Task<string> UrlPayment(int TypePaymentVN, int orderID)
        {
            var urlPayment = "";
            var order = await _orderRepository.GetById(orderID);

            //Get Config Info
            string vnp_Returnurl = _configuration.GetSection("VnpConfig:vnp_ReturnUrl").Value; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration.GetSection("VnpConfig:vnp_Url").Value; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration.GetSection("VnpConfig:vnp_TmnCode").Value; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = _configuration.GetSection("VnpConfig:vnp_HashSecret").Value; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var totalPayment = _orderRepository.TotalPayment(order);
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (totalPayment * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.OrderDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.ID);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.ID.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion
    }
}
