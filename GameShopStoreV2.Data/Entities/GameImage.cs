using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class GameImage
    {
        public int ImageId { get; set; }
        public int GameId { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Caption { get; set; } = null!;
        public bool isDefault { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SortOrder { get; set; }
        public long Filesize { get; set; }
        public Game Game { get; set; } = null!;
    }
}
