using FashionShop.Models.Domain;
using FashionShop.Models.DTO.UserDTO;
using FashionShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UsersController(UserManager<User> userManager, IUserRepository userRepository) 
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet("get-list-users")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetListUsers(string? searchByName, string? filterRole)
        {
            try
            {
                var listUsers = await _userRepository.GetAllUser(searchByName, filterRole);

                return Ok(listUsers);
            }
            catch
            {
                return BadRequest("Lấy danh sách người dùng không thành công");
            }
        }

        [HttpGet("get-user-by-id/{id}")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);

                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("register-admin")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> RegisterAdmin (RegisterRequestDTO registerAdminRequestDTO)
        {
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(registerAdminRequestDTO.Email);
                if(checkEmail != null)
                {
                    return BadRequest("Email đã tồn tại, vui lòng nhập email khác");
                }

                if (registerAdminRequestDTO.Password != registerAdminRequestDTO.RePassword)
                {
                    return BadRequest("Mật khẩu và nhập lại mật khẩu không khớp");
                }

                var accountAdmin = await _userRepository.RegisterAccountAdmin(registerAdminRequestDTO);
                if (accountAdmin == true)
                {
                    return Ok("Tạo tài khoản admin thành công");
                }
                else 
                {
                    return BadRequest("Tạo tài khoản admin không thành công");
                }
            }
            catch
            {
                return BadRequest("Tạo tài khoản admin không thành công");
            }
        }

        [HttpPost("register-member")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> RegisterMember(RegisterRequestDTO registerAdminRequestDTO)
        {
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(registerAdminRequestDTO.Email);
                if (checkEmail != null)
                {
                    return BadRequest("Email đã tồn tại, vui lòng nhập email khác");
                }

                if (registerAdminRequestDTO.Password != registerAdminRequestDTO.RePassword)
                {
                    return BadRequest("Mật khẩu và nhập lại mật khẩu không khớp");
                }

                var accountMember = await _userRepository.RegisterAccountMember(registerAdminRequestDTO);
                if (accountMember == true)
                {
                    return Ok("Tạo tài khoản nhân viên thành công");
                }
                else
                {
                    return BadRequest("Tạo tài khoản nhân viên không thành công");
                }
            }
            catch
            {
                return BadRequest("Tạo tài khoản nhân viên không thành công");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var loginAccount = await _userRepository.Login(loginRequestDTO);

                if(loginAccount != null)
                {
                    return Ok(loginAccount);
                }
                else
                {
                    return BadRequest("Đăng nhập không thành công");
                }
            }
            catch
            {
                return BadRequest("Đăng nhập không thành công");
            }
        }

        [HttpPut("account-lock/{idAccount}")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> AccountLock(string idAccount)
        {
            try
            {
                var result = await _userRepository.AccountLock(idAccount);
                if (result == true)
                {
                    return Ok("Khóa tài khoản thành công");
                }
                else
                {
                    return BadRequest("Tài khoản này đã bị khóa");
                }
            }
            catch
            {
                return BadRequest("Thực hiện khóa không thành công");
            }
        }

        [HttpPut("account-unlock/{idAccount}")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> AccountUnLock(string idAccount)
        {
            try
            {
                var result = await _userRepository.AccountUnlock(idAccount);
                if (result == true)
                {
                    return Ok("Mở khóa tài khoản thành công");
                }
                else
                {
                    return BadRequest("Tài khoản này không bị khóa");
                }
            }
            catch
            {
                return BadRequest("Thực hiện mở khóa không thành công");
            }
        }

        [HttpPut("update-user/{id}")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDTO, string id)
        {
            try
            {
                var userById = await _userManager.FindByIdAsync(id);
                var checkEmail = await _userManager.FindByEmailAsync(updateUserDTO.Email);
                
                if (checkEmail != null && checkEmail.Email != userById.Email)
                {
                    return BadRequest("Email đã tồn tại, vui lòng nhập email khác");
                }

                var updateUser = await _userRepository.Update(updateUserDTO, id);
                if (updateUser != null)
                {
                    return Ok("Chỉnh sửa tài khoản thành công");
                }
                else
                {
                    return BadRequest("Chỉnh sửa tài khoản không thành công");
                }
            }
            catch
            {
                return BadRequest("Chỉnh sửa tài khoản không thành công");

            }
        }

        [HttpDelete("delete-user/{id}")]
        [AuthorizeRoles("Quản trị viên")]
        public async Task<IActionResult> DeleleUser(string id)
        {
            try
            {
                var user = await _userRepository.Delete(id);
                if (user == true)
                {
                    return Ok("Xóa người dùng thành công");
                }
                else
                {
                    return BadRequest("Không tìm thấy id của người dùng");
                }
            }
            catch
            {
                return BadRequest("Xóa người dùng không thành công");
            }
        }

        [HttpGet("get-count-customer")]
        [AuthorizeRoles("Quản trị viên", "Nhân viên")]
        public async Task<IActionResult> GetCountCustomer()
        {
            try
            {
                var count = await _userRepository.Count();

                return Ok(count);
            }
            catch
            {
                return BadRequest("Lỗi");
            }
        }
    }
}
