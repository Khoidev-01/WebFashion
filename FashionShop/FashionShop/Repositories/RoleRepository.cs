using FashionShop.Data;
using FashionShop.Models.DTO.RolesDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Repositories
{

    public interface IRoleRepository
    {
        public Task<List<GetRoleDTO>> GetAll();
        public Task<CreateRoleDTO> Create(CreateRoleDTO createRoleDTO);
        public Task<GetRoleDTO> GetById(string id);
        public Task<CreateRoleDTO> Update(CreateRoleDTO createRoleDTO, string id);
        public Task<IdentityRole> Delete(string id);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly FashionShopDBContext _fashionShopDBContext;

        public RoleRepository(FashionShopDBContext fashionShopDBContext)
        {
            _fashionShopDBContext = fashionShopDBContext;
        }
        public async Task<List<GetRoleDTO>> GetAll()
        {
            var listRoleDomain = await _fashionShopDBContext.Roles.Select(role => new GetRoleDTO()
            {
                ID = role.Id,
                Name = role.Name,
            }).ToListAsync();

            return listRoleDomain;
        }

        public async Task<GetRoleDTO> GetById(string id)
        {
            var roleDomainById = await _fashionShopDBContext.Roles.Select(role => new GetRoleDTO()
            {
                ID = role.Id,
                Name = role.Name
            }).FirstOrDefaultAsync(role => role.ID == id);

            return roleDomainById;
        }

        public async Task<CreateRoleDTO> Create(CreateRoleDTO createRoleDTO)
        {
            var createRoleDomain = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = new Guid().ToString(),
                Name = createRoleDTO.Name,
                NormalizedName = createRoleDTO.Name.ToUpper(),
            };

            await _fashionShopDBContext.Roles.AddAsync(createRoleDomain);
            await _fashionShopDBContext.SaveChangesAsync();

            return createRoleDTO;
        }

        public async Task<CreateRoleDTO> Update(CreateRoleDTO createRoleDTO, string id)
        {
            var updateRoleDomain = await _fashionShopDBContext.Roles.FirstOrDefaultAsync(role => role.Id == id);
            if (updateRoleDomain != null)
            {
                updateRoleDomain.Name = createRoleDTO.Name;
                updateRoleDomain.NormalizedName = createRoleDTO.Name.ToUpper();

                await _fashionShopDBContext.SaveChangesAsync();
            }
            else
            {
                return null!;
            }
            return createRoleDTO;
        }

        public async Task<IdentityRole> Delete(string id)
        {
            var deleteRoleDomain = await _fashionShopDBContext.Roles.FirstOrDefaultAsync(role => role.Id == id);
            if (deleteRoleDomain != null)
            {
                _fashionShopDBContext.Roles.Remove(deleteRoleDomain);
                await _fashionShopDBContext.SaveChangesAsync();
            }
            else
            {
                return null!;
            }

            return deleteRoleDomain;
        }
    }
}
