using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Carts
{
    public class ResponseOrderedItem
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public DateTime DateAdded { get; set; }
        public List<string> ImageList { get; set; } = null!;
        public List<string> GenreName { get; set; } = null!;
        public List<int> GenreIds { get; set; } = null!;
    }
}
