using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Stats
{
    public class CountGameBuysViewModel
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public int BuyCount { get; set; }
        public decimal Total { get; set; }
    }
}
