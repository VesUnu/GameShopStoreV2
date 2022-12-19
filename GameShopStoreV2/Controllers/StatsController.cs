using GameShopStoreV2.Application.ItemServices.Stats;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("BestSellerPerMonthSortbyBuy/{year}/{month}/{take}")]
        public async Task<IActionResult> GameStatisticalByMonthAndYearSortbyBuy(int year, int month, int take)
        {
            var result = await _statsService.GameStatisticalByMonthAndYear(year, month, take);
            return Ok(result);
        }

        [HttpGet("BestSellerPerMonthSortbyTotal/{year}/{month}/{take}")]
        public async Task<IActionResult> GameStatisticalByMonthAndYearSortbyToTal(int year, int month, int take)
        {
            var result = await _statsService.GameStatisticalByMonthAndYearSortbyTotal(year, month, take);
            return Ok(result);
        }

        [HttpGet("TotalProfit")]
        public async Task<IActionResult> TotalProfit()
        {
            var result = await _statsService.TotalProfit();
            return Ok(result);
        }

        [HttpGet("GameTotalPurchased")]
        public async Task<IActionResult> GameTotalPurchased()
        {
            var result = await _statsService.GameTotalPurchased();
            return Ok(result);
        }
    }
}
