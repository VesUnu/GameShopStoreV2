using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class OrderedGame
    {
        public int OrderId { get; set; }
        public Game Game { get; set; } = null!;
        public Cart Cart { get; set; } = null!;
        public int CartId { get; set; }
        public int GameId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
