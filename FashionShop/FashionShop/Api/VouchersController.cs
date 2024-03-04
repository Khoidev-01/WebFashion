using FashionShop.Models.DTO.ProductDTO;
using FashionShop.Models.DTO.VoucherDTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepository;

        public VouchersController(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        [HttpGet("get-list-vouchers")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetListVouchers()
        {
            try
            {
                var listVouchers = await _voucherRepository.GetAll();

                return Ok(listVouchers);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("get-voucher-by-id/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetVoucherById(int id)
        {
            try
            {
                var voucher = await _voucherRepository.GetById(id);

                if (voucher != null)
                {
                    return Ok(voucher);
                }
                else
                {
                    return BadRequest("Không tìm thấy id của voucher");
                }

            }
            catch
            {
                return BadRequest("Lấy voucher theo id không thành công");
            }
        }

        [HttpPost("create-voucher")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> CreateVoucher(CreateVoucherDTO createVoucherDTO)
        {
            try
            {
                if(createVoucherDTO.DiscountAmount == true && createVoucherDTO.DiscountValue <= 0)
                {
                    return BadRequest("Số tiền giảm phải lớn hơn 0");
                }

                if (createVoucherDTO.DiscountPercentage == true && (createVoucherDTO.DiscountValue <= 0 || createVoucherDTO.DiscountValue > 100))
                {
                    return BadRequest("Phần trăm giảm phải nằm trong khoảng 1 đến 100");
                }

                if (createVoucherDTO.EndDate <= createVoucherDTO.StartDate)
                {
                    return BadRequest("Ngày bắt đầu và kết thúc không hợp lệ");
                }

                var voucher = await _voucherRepository.Create(createVoucherDTO);

                if (voucher != null)
                {
                    return Ok(voucher);
                }
                else
                {
                    return BadRequest("Tạo voucher không thành công");
                }

            }
            catch
            {
                return BadRequest("Mã giảm giá đã tồn tại");
            }
        }

        [HttpPut("update-voucher/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> UpdateProduct(UpdateVoucherDTO updateVoucherDTO, int id)
        {
            try
            {
                if (updateVoucherDTO.DiscountAmount == true && updateVoucherDTO.DiscountValue <= 0)
                {
                    return BadRequest("Số tiền giảm phải lớn hơn 0");
                }

                if (updateVoucherDTO.DiscountPercentage == true && (updateVoucherDTO.DiscountValue <= 0 || updateVoucherDTO.DiscountValue > 100))
                {
                    return BadRequest("Phần trăm giảm phải nằm trong khoảng 1 đến 100");
                }

                if (updateVoucherDTO.EndDate <= updateVoucherDTO.StartDate)
                {
                    return BadRequest("Ngày bắt đầu và kết thúc không hợp lệ");
                }

                var voucher = await _voucherRepository.Update(updateVoucherDTO, id);
                if (voucher != null)
                {
                    return Ok(voucher);
                }
                else
                {
                    return BadRequest("Không tìm thấy id của voucher");
                }
            }
            catch
            {
                return BadRequest("Mã giảm giá đã tồn tại");
            }
        }

        [HttpDelete("delete-voucher/{id}")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> DeleleVoucher(int id)
        {
            try
            {
                var voucher = await _voucherRepository.Delete(id);
                if (voucher == true)
                {
                    return Ok("Xóa voucher thành công");
                }
                else
                {
                    return BadRequest("Không tìm thấy id của voucher");
                }
            }
            catch
            {
                return BadRequest("Xóa voucher không thành công");
            }
        }

        [HttpGet("get-count-voucher")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetCountVoucher()
        {
            try
            {
                var count = await _voucherRepository.Count();

                return Ok(count);
            }
            catch
            {
                return BadRequest("Lỗi");
            }
        }
    }
}
