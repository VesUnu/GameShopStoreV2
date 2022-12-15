using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.System.Users
{
    public class AccountConfirmRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string CodeConfirm { get; set; } = null!;
    }
}
