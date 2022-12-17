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
    public class GameImageConfig : IEntityTypeConfiguration<GameImage>
    {
        public void Configure(EntityTypeBuilder<GameImage> builder)
        {
            builder.ToTable("GameImages");
            builder.HasKey(x => x.ImageId);
            builder.Property(x => x.ImageId).UseIdentityColumn();
            builder.HasOne(x => x.Game).WithMany(x => x.GameImages).HasForeignKey(x => x.GameId);
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Caption).HasMaxLength(200).IsRequired();
        }
    }
}
