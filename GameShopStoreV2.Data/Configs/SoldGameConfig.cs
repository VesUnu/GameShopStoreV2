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
    public class SoldGameConfig : IEntityTypeConfiguration<SoldGame>
    {
        public void Configure(EntityTypeBuilder<SoldGame> builder)
        {
            builder.ToTable("SoldGames");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Checkout)
                .WithMany(x => x.SoldGames)
                .HasForeignKey(x => x.CheckoutId);
        }
    }
}
