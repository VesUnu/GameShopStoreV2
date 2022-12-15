using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Games
{
    public class CreatedGenreRequest
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; } = null!;
    }
}
