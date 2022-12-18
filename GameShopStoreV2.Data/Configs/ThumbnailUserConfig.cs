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
    public class ThumbnailUserConfig : IEntityTypeConfiguration<ThumbnailUser>
    {
        public void Configure(EntityTypeBuilder<ThumbnailUser> builder)
        {
            builder.ToTable("ThumbnailUser");
            builder.HasKey(x => x.ImageId);
            builder.Property(x => x.ImagePath).IsRequired();
            builder.HasOne(x => x.AppUser).WithOne(x => x.ThumbnailUser).HasForeignKey<ThumbnailUser>(x => x.UserId);
        }
    }
}
