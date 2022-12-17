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
    public class GameGenreConfig : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasKey(t => new { t.GenreId, t.GameId });
            builder.ToTable("GameGenre");
            builder.HasOne(t => t.Game).WithMany(gg => gg.GameGenres).HasForeignKey(gg => gg.GameId);
            builder.HasOne(t => t.Genre).WithMany(gg => gg.GameGenres).HasForeignKey(gg => gg.GenreId);
        }
    }
}
