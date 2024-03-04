using FashionShop.Data;
using FashionShop.Models.Domain;
using FashionShop.Models.DTO.ProductDTO;
using FashionShop.Models.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace FashionShop.Repositories
{
    public interface IUserRepository
    {
        public Task<List<GetUserDTO>> GetAllUser(string? searchByName, string? filterRole);
        public Task<GetUserDTO> GetUserById(string id);
        public Task<bool> RegisterAccountAdmin(RegisterRequestDTO registerRequestDTO);

        public Task<bool> RegisterAccountMember(RegisterRequestDTO registerRequestDTO);

        public Task<bool> RegisterAccountCustomer(RegisterRequestDTO registerRequestDTO);

        public Task<string> Login(LoginRequestDTO loginRequestDTO);

        public Task<bool> AccountLock(string idAccount);
        public Task<bool> AccountUnlock(string idAccount);
        public Task<UpdateUserDTO> Update(UpdateUserDTO updateUserDTO, string id);
        public Task<bool> Delete(string id);

        public Task<int> Count();
    }
    public class UserRepository : IUserRepository
    {
        private readonly FashionShopDBContext _fashionShopDBContext;
        private readonly UserManager<User> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(FashionShopDBContext fashionShopDBContext, UserManager<User> userManager, ITokenRepository tokenRepository) {
            _fashionShopDBContext = fashionShopDBContext;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        public async Task<List<GetUserDTO>> GetAllUser(string searchByName, string filterRole)
        {
            var listUsersDomain = _fashionShopDBContext.Users.AsQueryable();

            if(!searchByName.IsNullOrEmpty())
            {
                listUsersDomain = listUsersDomain.Where(u => u.FullName.Contains(searchByName));
            }

            if (!filterRole.IsNullOrEmpty())
            {
                listUsersDomain = listUsersDomain.Where(u => _fashionShopDBContext.UserRoles
                        .Where(ur => ur.RoleId == filterRole)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id)
                );
            }

            var listUsersDTO = await listUsersDomain.Select(user => new GetUserDTO
            {
                ID = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = _fashionShopDBContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_fashionShopDBContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .First(),
                LockoutEnabled = user.LockoutEnabled,
            }).OrderByDescending(u => u.ID).ToListAsync();

            return listUsersDTO;
        }

        public async Task<GetUserDTO> GetUserById (string id)
        {
            var userDomain = await _userManager.FindByIdAsync(id);

            if(userDomain != null)
            {
                var userDTO = new GetUserDTO
                {
                    ID = userDomain.Id,
                    FullName = userDomain.FullName,
                    Email = userDomain.Email,
                    PhoneNumber = userDomain.PhoneNumber,
                };

                return userDTO;
            }

            return null;
        }

        public async Task<bool> RegisterAccountAdmin(RegisterRequestDTO registerRequestDTO)
        {
            var admin = new User
            {
                FullName = registerRequestDTO.FullName,
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                PhoneNumber = registerRequestDTO.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(admin, registerRequestDTO.Password);


            if(result.Succeeded) {
                result = await _userManager.AddToRoleAsync(admin, "Quản trị viên");
            
                if(result.Succeeded)
                {
                    admin.LockoutEnabled = false;
                    await _fashionShopDBContext.SaveChangesAsync();

                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RegisterAccountMember(RegisterRequestDTO registerRequestDTO)
        {
            var admin = new User
            {
                FullName = registerRequestDTO.FullName,
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                PhoneNumber = registerRequestDTO.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(admin, registerRequestDTO.Password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(admin, "Nhân viên");

                if (result.Succeeded)
                {
                    admin.LockoutEnabled = false;
                    await _fashionShopDBContext.SaveChangesAsync();

                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RegisterAccountCustomer(RegisterRequestDTO registerRequestDTO)
        {
            var admin = new User
            {
                FullName = registerRequestDTO.FullName,
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                PhoneNumber = registerRequestDTO.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(admin, registerRequestDTO.Password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(admin, "Khách hàng");

                if (result.Succeeded)
                {
                    admin.LockoutEnabled = false;
                    await _fashionShopDBContext.SaveChangesAsync();

                    return true;
                }
            }
            return false;
        }

        public async Task<string> Login(LoginRequestDTO loginRequestDTO)
        {
            var checkUser = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

            if(checkUser != null && checkUser.LockoutEnabled == false)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(checkUser, loginRequestDTO.Password);

                if (checkPassword)
                {
                    var roles = await _userManager.GetRolesAsync(checkUser);
                    if(roles != null && !roles.Contains("Khách hàng")) 
                    {
                        var jwtToken = _tokenRepository.CreateJWTToken(checkUser, roles.ToList());

                        return jwtToken;
                    }
                }
            }

            return null;
        }

        public async Task<bool> AccountLock(string idAccount)
        {
            var checkUser = await _userManager.FindByIdAsync(idAccount);

            if (checkUser != null && checkUser.LockoutEnabled == false)
            {
                checkUser.LockoutEnabled = true;
                _fashionShopDBContext.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<bool> AccountUnlock(string idAccount)
        {
            var checkUser = await _userManager.FindByIdAsync(idAccount);

            if (checkUser != null && checkUser.LockoutEnabled == true)
            {
                checkUser.LockoutEnabled = false;
                _fashionShopDBContext.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<UpdateUserDTO> Update(UpdateUserDTO updateUserDTO, string id)
        {
            var userDomain = await _fashionShopDBContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userDomain != null)
            {
                userDomain.FullName = updateUserDTO.FullName;
                userDomain.Email = updateUserDTO.Email;
                userDomain.PhoneNumber = updateUserDTO.PhoneNumber;

                await _fashionShopDBContext.SaveChangesAsync();
                return updateUserDTO;
            }
            else
            {
                return null!;
            }
        }

        public async Task<bool> Delete(string id)
        {
            var userDomain = await _fashionShopDBContext.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (userDomain != null)
            {
                _fashionShopDBContext.Remove(userDomain);
                await _fashionShopDBContext.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> Count()
        {
            var countCustomer = await _fashionShopDBContext.UserRoles.Where(ur => ur.RoleId == "9cd0f7a2-741d-405a-a8a3-a34b22da200c").ToListAsync();
            return countCustomer.Count();
        }
    }
}
