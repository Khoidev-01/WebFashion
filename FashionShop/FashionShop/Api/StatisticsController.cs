using FashionShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace FashionShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeRoles("Quản trị viên", "Nhân viên")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticRepository _statisticRepository;

        public StatisticsController(IStatisticRepository statisticRepository) 
        {
            _statisticRepository = statisticRepository;
        }

        [HttpGet("get-revenueStatistics")] 
        public IActionResult GetRevenueStatistic(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var revenueStatistics = _statisticRepository.GetRevenueStatistic(fromDate, toDate);

                return Ok(revenueStatistics);
            }
            catch
            {
                return BadRequest();
            }
            
        }
    }
}
