using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Games
{
    public class ManageGamePagingRequest
    {
        public string Keyword { get; set; } = null!;
        public int? GenreID { get; set; }
    }
}
