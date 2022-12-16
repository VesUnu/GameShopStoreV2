using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public List<Cart> Carts { get; set; } = null!;
        public Wishlist Wishlist { get; set; }
        public AvatarUser AvatarUser { get; set; } = null!;
        public ThumbnailUser ThumbnailUser { get; set; } = null!;
        public bool isConfirmed { get; set; }
        public string CodeConfirm { get; set; } = null!;
    }
}
