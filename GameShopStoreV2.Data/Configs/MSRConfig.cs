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
    public class MSRConfig : IEntityTypeConfiguration<MinSystemRequirement>
    {
        public void Configure(EntityTypeBuilder<MinSystemRequirement> builder)
        {
            builder.HasKey(x => x.SRMId);
            builder.HasOne(sr => sr.Game)
                   .WithOne(x => x.MinSystemRequirement)
                   .HasForeignKey<MinSystemRequirement>(sr => sr.GameID);
        }
    }
}
