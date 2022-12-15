using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameShopStoreV2.Core.System.Users
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "LastName")]
        public string LastName { get; set; } = null!;

        [Display(Name = "BirthDate")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;
    }
}
