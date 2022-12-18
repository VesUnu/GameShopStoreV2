using GameShopStoreV2.Core.Items.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Stats
{
    public interface IStatsService
    {
        Task<List<CountGameBuysViewModel>> GameStatisticalByMonthAndYear(int year, int month, int take);

        Task<List<CountGameBuysViewModel>> GameStatisticalByMonthAndYearSortbyTotal(int year, int month, int take);

        Task<decimal> TotalProfit();

        Task<List<CountGameBuysViewModel>> GameTotalPurchased();
    }
}
