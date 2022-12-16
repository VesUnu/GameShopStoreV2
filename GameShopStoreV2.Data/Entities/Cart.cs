using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class Cart : BaseEntity
    {
        public int CartId { get; set; }
        public List<OrderedGame> OrderedGames { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public Guid UserId { get; set; }
        public Checkout Checkout { get; set; } = null!;
        public int CheckoutId { get; set; }
    }
}
