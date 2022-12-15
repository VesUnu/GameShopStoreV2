using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Common
{
    public class BasePagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
