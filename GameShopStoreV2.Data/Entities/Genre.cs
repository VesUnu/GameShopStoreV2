using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class Genre
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; } = null!;
        public List<GameGenre> GameGenres { get; set; } = null!;
    }
}
