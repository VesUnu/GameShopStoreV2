using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class Game : BaseEntity
    {
        public int GameId { get; set; }
        public string GameName { get; set; } = null!;
        public Decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; } = null!;
        public string Gameplay { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public List<GameGenre> GameGenres { get; set; } = null!;
        public SystemRequireMin SystemRequireMin { get; set; }
        public SystemRequiredRecommended SystemRequiredRecommended { get; set; }
        public List<GameImage> GameImages { get; set; } = null!;
        public List<OrderedGame> OrderedGames { get; set; } = null!;
        public List<WishesGame> WishesGames { get; set; } = null!;
        public string FilePath { get; set; } = null!;
    }
}
