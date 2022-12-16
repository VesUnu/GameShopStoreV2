using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class SoldGame
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public Checkout Checkout { get; set; } = null!;
        public int CheckoutId { get; set; }
        public string ImagePath { get; set; } = null!;
        public string GameFile { get; set; } = null!;
        public DateTime DatePurchased { get; set; }
    }
}
