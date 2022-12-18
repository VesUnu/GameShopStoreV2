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
    public class AvatarUserConfig : IEntityTypeConfiguration<AvatarUser>
    {
        public void Configure(EntityTypeBuilder<AvatarUser> builder)
        {
            builder.ToTable("AvatarUser");
            builder.HasKey(x => x.ImageId);
            builder.Property(x => x.ImagePath).IsRequired();
            builder.HasOne(x => x.AppUser).WithOne(x => x.AvatarUser).HasForeignKey<AvatarUser>(x => x.UserId);
        }
    }
}
