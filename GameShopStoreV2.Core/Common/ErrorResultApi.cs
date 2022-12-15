using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Common
{
    public class ErrorResultApi<T> : ResultApi<T>
    {
        public string[] ValidationErrors { get; set; } = null!;

        public ErrorResultApi()
        {

        }

        public ErrorResultApi(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public ErrorResultApi(string[] validationErrors)
        {
            IsSuccess = false;
            ValidationErrors = validationErrors;
        }
    }
}
