using FashionShop.Data;
using FashionShop.Models.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FashionShop.Repositories
{
    public interface IStatisticRepository
    {
        public List<RevenueStatisticViewModel> GetRevenueStatistic(DateTime fromDate, DateTime toDate);
    }
    public class StatisticRepository : IStatisticRepository
    {
        private readonly FashionShopDBContext _fashionShopDBContext;

        public StatisticRepository(FashionShopDBContext fashionShopDBContext)
        {
            _fashionShopDBContext = fashionShopDBContext;
        }
        public List<RevenueStatisticViewModel> GetRevenueStatistic(DateTime fromDate, DateTime toDate)
        {
            var query = _fashionShopDBContext.Set<RevenueStatisticViewModel>()
                .FromSqlRaw("EXEC GetRevenueStatistic @fromDate, @toDate",
                    new SqlParameter("@fromDate", fromDate),
                    new SqlParameter("@toDate", toDate));

            return query.ToList();
        }
    }
}
