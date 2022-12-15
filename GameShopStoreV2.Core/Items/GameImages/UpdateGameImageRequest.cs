using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.GameImages
{
    public class UpdateGameImageRequest
    {
        public string Caption { get; set; } = null!;
        public bool isDefault { get; set; }
        public int SortOrder { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}
