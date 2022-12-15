using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Games
{
    public class BestSellerGame
    {
        public int GameID { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; } = null!;
        public string Gameplay { get; set; } = null!;
        public List<string> GenreName { get; set; } = null!;
        public List<int> GenreIds { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public SystemRequireMin SystemRequireMin { get; set; } = null!;
        public SystemRequiredRecommend SystemRequiredRecommend { get; set; } = null!;
        public List<string> ListImage { get; set; } = null!;
        public int BuyCount { get; set; }
        public string Publisher { get; set; } = null!;
    }
}
