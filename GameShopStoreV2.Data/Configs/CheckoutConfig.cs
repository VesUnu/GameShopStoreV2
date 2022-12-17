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
    public class CheckoutConfig : IEntityTypeConfiguration<Checkout>
    {
        public void Configure(EntityTypeBuilder<Checkout> builder)
        {
            builder.ToTable("Checkouts");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Cart)
               .WithOne(x => x.Checkout)
               .HasForeignKey<Checkout>(x => x.CartId);
            builder.Property(x => x.DatePurchased).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired();
            builder.Property(x => x.Username).IsRequired();
        }
    }
}
