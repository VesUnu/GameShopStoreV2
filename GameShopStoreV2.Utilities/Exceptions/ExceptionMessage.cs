using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Utilities.Exceptions
{
    public class ExceptionMessage : Exception
    {
        public ExceptionMessage()
        {

        }

        public ExceptionMessage(string message) 
            : base(message)
        {

        }

        public ExceptionMessage(string message, Exception inner) 
            : base(message, inner)
        {

        }
    }
}
