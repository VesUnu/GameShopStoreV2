using GameShopStoreV2.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Games
{
    public class AssignCategory
    {
        public int Id { get; set; }
        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();
    }
}
