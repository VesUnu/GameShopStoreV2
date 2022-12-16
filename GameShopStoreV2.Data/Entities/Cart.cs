using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class Cart : BaseEntity
    {
        public int CartID { get; set; }
        public List<OrderedGame> OrderedGames { get; set; }
        public AppUser AppUser { get; set; }
        public Guid UserID { get; set; }
        public Checkout Checkout { get; set; }
        public int CheckoutID { get; set; }
    }
}
