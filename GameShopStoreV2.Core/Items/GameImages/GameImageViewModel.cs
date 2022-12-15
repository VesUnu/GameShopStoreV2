using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.GameImages
{
    public class GameImageViewModel
    {
        public int GameId { get; set; }
        public int ImageId { get; set; }
        public string Path { get; set; } = null!;
        public bool isDefault { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FileSize { get; set; }
        public string Caption { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
