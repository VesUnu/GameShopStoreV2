using GameShopStoreV2.Core.Items.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Checkouts
{
    public class CheckoutViewModel
    {
        public int CartId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DatePurchased { get; set; }
        public string Username { get; set; } = null!;
        public List<GameViewModel> Listgame { get; set; } = null!;
    }
}
