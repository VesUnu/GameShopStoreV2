﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.Items.Games
{
    public class EditGameRequest
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public string Gameplay { get; set; } = null!;
        public int Status { get; set; }
        public IFormFile ThumbnailImage { get; set; } = null!;
        public SystemRequireMin SystemRequireMin { get; set; } = new SystemRequireMin();
        public SystemRequiredRecommend SystemRequiredRecommend { get; set; } = new SystemRequiredRecommend();
        public IFormFile FileGame { get; set; } = null!;
    }
}
