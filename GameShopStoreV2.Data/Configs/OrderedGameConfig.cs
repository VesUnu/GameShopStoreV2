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
    public class OrderedGameConfig : IEntityTypeConfiguration<OrderedGame>
    {
        public void Configure(EntityTypeBuilder<OrderedGame> builder)
        {
            builder.ToTable("OrderedGames");
            builder.HasKey(x => x.OrderId);
            builder.HasOne(x => x.Cart).WithMany(x => x.OrderedGames).HasForeignKey(x => x.CartId);
            builder.HasOne(x => x.Game).WithMany(x => x.OrderedGames).HasForeignKey(x => x.GameId);
        }
    }
}
