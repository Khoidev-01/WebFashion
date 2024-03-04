using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO.OrderDTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;
using System.Net.Mime;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("get-list-orders")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public IActionResult GetAll(int page = 0, int pageSize = 6, int? typePayment = null, int? searchByID = null, string? searchByName = null, string? searchBySDT = null)
        {
            try
            {
                var listOrders = _orderRepository.GetAll(page, pageSize, typePayment, searchByID, searchByName, searchBySDT);            

                return Ok(listOrders);
            }
            catch
            {
                return BadRequest("Lấy danh sách người dùng không thành công");
            }
        }

        [HttpGet("get-totalPayment-by-id/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public IActionResult TotalPayment(int id)
        {
            try
            {
                var totalPayment = _orderRepository.AdminTotalPayment(id);

                return Ok(totalPayment);
            }
            catch
            {
                return BadRequest("Lỗi");
            }
        }

        [HttpGet("get-order-by-id/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderRepository.GetById(id);

                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return BadRequest("Không tìm thấy id của order");
                }

            }
            catch
            {
                return BadRequest("Lấy product theo id không thành công");
            }
        }

        [HttpGet("export-excel/{id}")]
        public async Task<IActionResult> ExportExcel(int id)
        {
            var order = await _orderRepository.GetById(id);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                worksheet.Column(2).Width = 36;
                worksheet.Column(3).Width = 13;
                worksheet.Column(4).Width = 13;
                worksheet.Column(5).Width = 13;

                worksheet.Cells["A1:B3"].Merge = true;
                worksheet.Cells["A1:B3"].Value = "FASHION SHOP";
                worksheet.Cells["A1:B3"].Style.Font.Bold = true;
                worksheet.Cells["A1:B3"].Style.Font.Size = 14;
                worksheet.Cells["A1:B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:B3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["C1:E3"].Merge = true;
                worksheet.Cells["C1:E3"].Value = "HÓA ĐƠN BÁN HÀNG";
                worksheet.Cells["C1:E3"].Style.Font.Bold = true;
                worksheet.Cells["C1:E3"].Style.Font.Size = 14;
                worksheet.Cells["C1:E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["C1:E3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["A8:E8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A8:E8"].Style.Font.Bold = true;

                // Insert customer data into template
                worksheet.Cells[4, 1].Value = "Tên khách hàng: " + order.FullName;
                worksheet.Cells[5, 1].Value = "Địa chỉ: " + order.Address;
                worksheet.Cells[6, 1].Value = "Số điện thoại: " + order.PhoneNumber;

                // Start Row for Detail Rows
                worksheet.Cells[8, 1].Value = "STT";
                worksheet.Cells[8, 2].Value = "Tên Hàng";
                worksheet.Cells[8, 3].Value = "Giá";
                worksheet.Cells[8, 4].Value = "SL";
                worksheet.Cells[8, 5].Value = "TT";

                // load order Detail
                int rowIndex = 9;
                int count = 1;
                double totalMoney = 0;
                foreach (var item in order.OrderDetails)
                {
                    worksheet.Cells[rowIndex, 1].Value = count.ToString();
                    worksheet.Cells[rowIndex, 2].Value = item.Product.Name.ToString();

                    var price = item.Price / item.Quantity;
                    worksheet.Cells[rowIndex, 3].Value = price.ToString();
                    worksheet.Cells[rowIndex, 4].Value = item.Quantity.ToString();

                    var tt = item.Price * item.Quantity;
                    worksheet.Cells[rowIndex, 5].Value = tt.ToString();

                    totalMoney += tt;
                    rowIndex++;
                    count++;
                }

                worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Merge = true;
                worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Value = "Tổng cộng";
                worksheet.Cells[$"A{rowIndex}:B{rowIndex}"].Style.Font.Bold = true;
                worksheet.Cells[$"A9:B{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells[$"C8:E{rowIndex}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                worksheet.Cells[rowIndex, 5].Value = totalMoney.ToString();

                var cellRange = worksheet.Cells[$"A8:E{rowIndex}"];
                cellRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cellRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cellRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ++rowIndex;
                worksheet.Cells[rowIndex, 4].Value = "Vận chuyển: ";
                worksheet.Cells[rowIndex, 5].Value = order.DeliveryFee.ToString("C0", new CultureInfo("vi-VN"));
                worksheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ++rowIndex;
                worksheet.Cells[rowIndex, 4].Value = "Voucher: ";
                double voucherValue = 0;
                if (order.Voucher != null)
                {
                    if (order.Voucher.DiscountAmount == true)
                    {
                        voucherValue = order.Voucher.DiscountValue;
                        worksheet.Cells[rowIndex, 5].Value = "-" + voucherValue.ToString("C0", new CultureInfo("vi-VN"));
                    }
                    else
                    {
                        voucherValue = totalMoney * (order.Voucher.DiscountValue / 100);
                        worksheet.Cells[rowIndex, 5].Value = "-" + voucherValue.ToString("C0", new CultureInfo("vi-VN"));
                    }
                }
                else
                {
                    worksheet.Cells[rowIndex, 5].Value = "-" + 0.ToString("C0", new CultureInfo("vi-VN"));
                }

                worksheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                var totalPayment = totalMoney + order.DeliveryFee - voucherValue;

                ++rowIndex;
                worksheet.Cells[rowIndex, 4].Value = "Thành tiền: ";
                worksheet.Cells[rowIndex, 5].Value = totalPayment.ToString("C0", new CultureInfo("vi-VN"));
                worksheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                rowIndex += 3;
                worksheet.Cells[$"C{rowIndex}:E{rowIndex}"].Merge = true;
                var date = order.OrderDate;
                var dateFormat = "Ngày " + date.Day + " tháng " + date.Month + " năm " + date.Year;
                worksheet.Cells[rowIndex, 3].Value = dateFormat;
                worksheet.Cells[rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ++rowIndex;
                worksheet.Cells[$"C{rowIndex}:E{rowIndex}"].Merge = true;
                worksheet.Cells[rowIndex, 2].Value = "KHÁCH HÀNG";
                worksheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowIndex, 3].Value = "NGƯỜI BÁN HÀNG";
                worksheet.Cells[rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var contentType = MediaTypeNames.Application.Octet;
                var fileExtension = "xlsx";

                var contentDisposition = new ContentDisposition
                {
                    Inline = false,
                    FileName = $"HoaDon-{order.ID}.{fileExtension}"
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                var bytes = package.GetAsByteArray();

                return File(bytes, contentType, $"HoaDon-{order.ID}.{fileExtension}");
            }
        }

        [HttpGet("get-count-order")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetCountOrder()
        {
            try
            {
                var count = await _orderRepository.Count();

                return Ok(count);
            }
            catch
            {
                return BadRequest("Lỗi");
            }
        }
    }
}
