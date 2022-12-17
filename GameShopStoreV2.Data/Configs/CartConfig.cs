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
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(x => x.CartId);
            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserId)
                 .HasPrincipalKey(x => x.Id);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.DateUpdated).IsRequired();
        }
    }
}
