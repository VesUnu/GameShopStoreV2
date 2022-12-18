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
    public class WishesGamesConfig : IEntityTypeConfiguration<WishesGame>
    {
        public void Configure(EntityTypeBuilder<WishesGame> builder)
        {
            builder.ToTable("WishesGames");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Wishlist).WithMany(x => x.WishesGame).HasForeignKey(x => x.WishId);
            builder.HasOne(x => x.Game).WithMany(x => x.WishesGames).HasForeignKey(x => x.GameId);
        }
    }
}
