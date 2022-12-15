using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameShopStoreV2.Core.System.Users
{
    public class RegisterRequest
    {
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
