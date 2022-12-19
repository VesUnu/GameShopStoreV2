using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.System.Users
{
    public class LoginResponse
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public bool IsConfirmed { get; set; }
    }
}
