using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class Checkout
    {
        public int Id { get; set; }
        public Cart Cart { get; set; } = null!;
        public int CartId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DatePurchased { get; set; }
        public string Username { get; set; } = null!;
        public List<SoldGame> SoldGames { get; set; } = null!;
    }
}
