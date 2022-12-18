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


    }
}
