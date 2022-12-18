using GameShopStoreV2.Core.Items.Stats;
using GameShopStoreV2.Data.EF;
using GameShopStoreV2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Application.ItemServices.Stats
{
    public class StatsService : IStatsService
    {
        private readonly GameShopStoreVTwoDBContext _context;

        public StatsService(GameShopStoreVTwoDBContext context)
        {
            _context = context;
        }

        public async Task<List<CountGameBuysViewModel>> GameStatisticalByMonthAndYear(int year, int month, int take)
        {
            if (year < 2022 || year > DateTime.Today.Year)
            {
                year = DateTime.Today.Year;
            }
            if (month < 1 || month > 12)
            {
                month = DateTime.Today.Month;
            }
            var listCheckout = await _context.Checkouts
                .Where(x => x.DatePurchased.Year == year && x.DatePurchased.Month == month).Include(x => x.SoldGames).ToListAsync();
            var listGames = await _context.Games.Select(x => new CountGameBuysViewModel
            {
                Name = x.GameName,
                GameId = x.GameId,
                BuyCount = 0,
                Total = 0
            }).ToListAsync();
            if (take < 0 || take > listGames.Count)
            {
                take = listGames.Count;
            }
            var listSoldGame = new List<SoldGame>();
            foreach (var checkout in listCheckout)
            {
                var soldgames = checkout.SoldGames;
                foreach (var soldgame in soldgames)
                {
                    listSoldGame.Add(soldgame);
                }
            }
            foreach (var game in listGames)
            {
                foreach (var soldgame in listSoldGame)
                {
                    decimal totalprice = 0;
                    if (game.GameId == soldgame.GameId)
                    {
                        game.BuyCount++;
                        totalprice = totalprice + (soldgame.Price - (soldgame.Price * soldgame.Discount / 100));
                        game.Total += totalprice;
                    }
                }
            }
            return listGames.OrderByDescending(x => x.BuyCount).Take(take).ToList();
        }

        public async Task<List<CountGameBuysViewModel>> GameStatisticalByMonthAndYearSortbyTotal(int year, int month, int take)
        {
            if (year < 2022 || year > DateTime.Today.Year)
            {
                year = DateTime.Today.Year;
            }
            if (month < 1 || month > 12)
            {
                month = DateTime.Today.Month;
            }
            var listCheckout = await _context.Checkouts
                .Where(x => x.DatePurchased.Year == year && x.DatePurchased.Month == month).Include(x => x.SoldGames).ToListAsync();
            var listGames = await _context.Games.Select(x => new CountGameBuysViewModel
            {
                Name = x.GameName,
                GameId = x.GameId,
                BuyCount = 0,
                Total = 0
            }).ToListAsync();
            if (take < 0 || take > listGames.Count)
            {
                take = listGames.Count;
            }
            var listSoldGame = new List<SoldGame>();
            foreach (var checkout in listCheckout)
            {
                var soldgames = checkout.SoldGames;
                foreach (var soldgame in soldgames)
                {
                    listSoldGame.Add(soldgame);
                }
            }
            foreach (var game in listGames)
            {
                foreach (var soldgame in listSoldGame)
                {
                    decimal totalprice = 0;
                    if (game.GameId == soldgame.GameId)
                    {
                        game.BuyCount++;
                        totalprice = totalprice + (soldgame.Price - (soldgame.Price * soldgame.Discount / 100));
                        game.Total += totalprice;
                    }
                }
            }
            return listGames.OrderByDescending(x => x.Total).Take(take).ToList();
        }

        public async Task<List<CountGameBuysViewModel>> GameTotalPurchased()
        {
            var listGames = await _context.Games.Select(x => new CountGameBuysViewModel
            {
                Name = x.GameName,
                GameId = x.GameId,
                BuyCount = 0,
                Total = 0
            }).ToListAsync();
            var soldgames = await _context.SoldGames.ToListAsync();
            foreach (var game in listGames)
            {
                foreach (var soldgame in soldgames)
                {
                    decimal totalprice = 0;
                    if (game.GameId == soldgame.GameId)
                    {
                        game.BuyCount++;
                        totalprice = totalprice + (soldgame.Price - (soldgame.Price * soldgame.Discount / 100));
                        game.Total += totalprice;
                    }
                }
            }
            return listGames;
        }

        public async Task<decimal> TotalProfit()
        {
            var listCheckOut = await _context.Checkouts.ToListAsync();
            decimal total = 0;
            foreach (var checkout in listCheckOut)
            {
                total += checkout.TotalPrice;
            }
            return total;
        }
    }
}
