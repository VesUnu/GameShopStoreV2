using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public List<WishesGame> WishesGame { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
