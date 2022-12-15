using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Common
{
    public class SelectItem
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public bool Selected { get; set; }

        public object Select() 
        {
            throw new NotImplementedException();
        }
    }
}
