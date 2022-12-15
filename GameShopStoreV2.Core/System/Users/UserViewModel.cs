using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.System.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "LastName")]
        public string LastName { get; set; } = null!;
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        public RoleAssignRequest RoleAssign { get; set; }
        public IList<string> Roles { get; set; } = null!;
        public string AvatarPath { get; set; } = null!;
        public string ThumbnailPath { get; set; } = null!;
    }
}
