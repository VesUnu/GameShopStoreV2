using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Entities
{
    public class AvatarUser
    {
        public int ImageId { get; set; }
        public AppUser AppUser { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime DateUpdate { get; set; }
        public string ImagePath { get; set; } = null!;
        }
}
