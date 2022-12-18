using GameShopStoreV2.Data.Configs;
using GameShopStoreV2.Data.Entities;
using GameShopStoreV2.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.EF
{
    public class GameShopStoreVTwoDBContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public GameShopStoreVTwoDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfig());
            modelBuilder.ApplyConfiguration(new GameGenreConfig());
            modelBuilder.ApplyConfiguration(new MSRConfig());
            modelBuilder.ApplyConfiguration(new RSRConfig());
            modelBuilder.ApplyConfiguration(new GameImageConfig());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            modelBuilder.ApplyConfiguration(new CartConfig());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfig());
            modelBuilder.ApplyConfiguration(new OrderedGameConfig());
            modelBuilder.ApplyConfiguration(new WishlistConfig());
            modelBuilder.ApplyConfiguration(new WishesGameConfig());
            modelBuilder.ApplyConfiguration(new CheckoutConfig());
            modelBuilder.ApplyConfiguration(new ContactConfig());
            modelBuilder.ApplyConfiguration(new AvatarUserConfig());
            modelBuilder.ApplyConfiguration(new ThumbnailUserConfig());
            modelBuilder.ApplyConfiguration(new SoldGameConfig());
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("ApplicationUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("ApplicationUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("ApplicationUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("ApplicationRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("ApplicationUserTokens").HasKey(x => x.UserId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
        }

        public DbSet<GameGenre> GameGenres { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<MinSystemRequirement> MinSystemRequirements { get; set; } = null!;
        public DbSet<RecommendSystemRequirement> RecommendSystemRequirements { get; set; } = null!;
        public DbSet<GameImage> GameImages { get; set; } = null!;
        public DbSet<OrderedGame> OrderedGames { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;
        public DbSet<WishesGame> WishesGames { get; set; } = null!;
        public DbSet<Checkout> Checkouts { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<AvatarUser> AvatarUsers { get; set; } = null!;
        public DbSet<ThumbnailUser> ThumbnailUsers { get; set; } = null!;
        public DbSet<SoldGame> SoldGames { get; set; } = null!;
    }
}
