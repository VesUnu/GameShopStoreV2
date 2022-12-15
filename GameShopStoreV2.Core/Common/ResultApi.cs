using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Common
{
    public class ResultApi<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = null!;

        public T ResultObj { get; set; }
    }
}
