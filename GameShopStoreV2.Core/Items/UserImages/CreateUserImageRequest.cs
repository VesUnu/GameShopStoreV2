using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.UserImages
{
    public class CreateUserImageRequest
    {
        public IFormFile ImageFile { get; set; } = null!;
    }
}
