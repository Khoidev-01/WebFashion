using FashionShop.Data;
using FashionShop.Models.ViewModel;

namespace FashionShop.Repositories
{
    public interface IWardRepository
    {
        public List<WardViewModel> GetAll(int idProvince, int idDistrict);
    }
    public class WardRepository : IWardRepository
    {
        FashionShopDBContext _fashionShopDBContext;
        public WardRepository(FashionShopDBContext fashionShopDBContext)
        {
            _fashionShopDBContext = fashionShopDBContext;
        }

        public List<WardViewModel> GetAll(int idProvince, int idDistrict)
        {
            var listWards = _fashionShopDBContext.Wards
                .Where(w => w.ProvinceID == idProvince && w.DistrictID == idDistrict)
                .Select(w => new WardViewModel
                {
                    ID = w.ID,
                    Name = w.Name,
                    ProvinceID = w.ProvinceID,
                    DistrictID = w.DistrictID,
                }).ToList();

            return listWards;   
        }
    }
}
