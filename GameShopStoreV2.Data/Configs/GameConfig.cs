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
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");
            builder.HasKey(x => x.GameId);
            builder.Property(x => x.GameName).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Discount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Gameplay).IsRequired();
        }
    }
}
