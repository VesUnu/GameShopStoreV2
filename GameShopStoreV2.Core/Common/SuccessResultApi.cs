using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Common
{
    public class SuccessResultApi<T> : ResultApi<T>
    {
        public SuccessResultApi(T resultObj)
        {
            IsSuccess = true;
            ResultObj = resultObj;
        }
        public SuccessResultApi()
        {
            IsSuccess = true;
        }
    }
}
