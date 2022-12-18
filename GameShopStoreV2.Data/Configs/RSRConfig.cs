using GameShopStoreV2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Configs
{
    public class RSRConfig : IEntityTypeConfiguration<RecommendSystemRequirement>
    {
        public void Configure(EntityTypeBuilder<RecommendSystemRequirement> builder)
        {
            builder.HasKey(x => x.SRRId);
            builder.HasOne(sr => sr.Game)
                .WithOne(x => x.RecommendSystemRequirement)
                .HasForeignKey<RecommendSystemRequirement>(sr => sr.GameId);
        }
    }
}
