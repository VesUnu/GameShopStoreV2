using GameShopStoreV2.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder) 
        {
            //For data seeding
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 1,
                   GenreName = "Action"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 2,
                   GenreName = "Open-World"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 3,
                   GenreName = "Multiplayer"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 4,
                   GenreName = "Action RPG"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 5,
                   GenreName = "Simulation"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 6,
                   GenreName = "Horror"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 7,
                   GenreName = "Sports & Racing"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 8,
                   GenreName = "Role-Playing"
               });
            modelBuilder.Entity<Genre>().HasData(
               new Genre()
               {
                   GenreId = 9,
                   GenreName = "Visual Novel"
               });

            modelBuilder.Entity<Game>().HasData(
                new Game()
                {
                    GameId = 1,
                    GameName = "Grand Theft Auto V",
                    Price = 75,
                    Discount = 0,
                    Description = "Grand Theft Auto V for PC offers players the option to explore the award-winning world of Los Santos and Blaine County in resolutions of up to 4k and beyond, as well as the chance to experience the game running at 60 frames per second.",
                    Gameplay = "Basic open-world game with missions and other cool stuff",
                    DateCreated = DateTime.Now,
                    Status = Enums.Status.Active,
                    DateUpdated = DateTime.Now,
                });
            modelBuilder.Entity<Game>().HasData(
               new Game()
               {
                   GameId = 2,
                   GameName = "Red Dead Redemption 2",
                   Price = 88,
                   Discount = 20,
                   Description = "Winner of over 175 Game of the Year Awards and recipient of over 250 perfect scores, RDR2 is the epic tale of outlaw Arthur Morgan and the infamous Van der Linde gang, on the run across America at the dawn of the modern age. Also includes access to the shared living world of Red Dead Online.",
                   Gameplay = "An open-world, action-adventure game with great story",
                   DateCreated = DateTime.Now,
                   Status = Enums.Status.Active,
                   DateUpdated = DateTime.Now,
               });
            modelBuilder.Entity<Game>().HasData(
                new Game()
                {
                    GameId = 3,
                    GameName = "FIFA 22",
                    Price = 59,
                    Discount = 15,
                    Description = "Powered by Football, EA SPORTS FIFA 22 brings the game even closer to the real thing with fundamental gameplay advances and a new season of innovation across every mode.",
                    Gameplay = "Football-based game",
                    DateCreated = DateTime.Now,
                    Status = Enums.Status.Active,
                    DateUpdated = DateTime.Now,
                });
            modelBuilder.Entity<Game>().HasData(
                new Game()
                {
                    GameId = 4,
                    GameName = "Call of Duty: Modern Warfare II",
                    Price = 69,
                    Discount = 15,
                    Description = "Call of Duty: Modern Warfare II drops players into an unprecedented global conflict that features the return of the iconic Operators of Task Force 141.",
                    Gameplay = "Online-based first-person shooter game",
                    DateCreated = DateTime.Now,
                    Status = Enums.Status.Active,
                    DateUpdated = DateTime.Now,
                });
            modelBuilder.Entity<GameGenre>().HasData(
                new GameGenre() { GenreId = 1, GameId = 1 },
                new GameGenre() { GenreId = 2, GameId = 1 }
            );
            modelBuilder.Entity<GameGenre>().HasData(
                new GameGenre() { GenreId = 3, GameId = 2 },
                new GameGenre() { GenreId = 2, GameId = 2 }
            );
            modelBuilder.Entity<GameGenre>().HasData(
                new GameGenre() { GenreId = 7, GameId = 3 },
                new GameGenre() { GenreId = 5, GameId = 3 }
            );
            modelBuilder.Entity<GameGenre>().HasData(
                new GameGenre() { GenreId = 4, GameId = 4 },
                new GameGenre() { GenreId = 1, GameId = 4 }
            );
            modelBuilder.Entity<MinSystemRequirement>().HasData(
                new MinSystemRequirement()
                {
                    SRMId = 1,
                    OpSystem = "Windows 10 64 Bit, Windows 8.1 64 Bit, Windows 8 64 Bit, Windows 7 64 Bit Service Pack 1",
                    Processor = "Intel Core 2 Quad CPU Q6600 @ 2.40GHz (4 CPUs) / AMD Phenom 9850 Quad-Core Processor (4 CPUs) @ 2.5GHz",
                    Memory = "4 GB RAM",
                    Graphics = "NVIDIA 9800 GT 1GB / AMD HD 4870 1GB (DX 10, 10.1, 11)",
                    Storage = "72 GB available space",
                    AdditionalNotes = "",
                    GameID = 1,
                    SoundCard = "100% DirectX 10 compatible"
                });
            modelBuilder.Entity<MinSystemRequirement>().HasData(
              new MinSystemRequirement()
              {
                  SRMId = 2,
                  OpSystem = "Windows 7 SP1",
                  Processor = "Intel Core i5-2500K / AMD FX-6300",
                  Memory = "8 GB RAM",
                  Graphics = "Nvidia GeForce GTX 770 2GB / AMD Radeon R9 280",
                  Storage = "150 GB available space",
                  AdditionalNotes = "",
                  GameID = 2,
                  SoundCard = "Direct X Compatible"
              });
            modelBuilder.Entity<MinSystemRequirement>().HasData(
              new MinSystemRequirement()
              {
                  SRMId = 3,
                  OpSystem = "Windows 10 - 64-Bit",
                  Processor = "Intel Core i3-6100 @ 3.7GHz or AMD Athlon X4 880K @4GHz",
                  Memory = "8 GB RAM",
                  Graphics = "NVIDIA GTX 660 2GB or AMD Radeon HD 7850 2GB",
                  Storage = "50 GB available space",
                  AdditionalNotes = "",
                  GameID = 3,
                  SoundCard = "Direct X Compatible"
              });
            modelBuilder.Entity<MinSystemRequirement>().HasData(
             new MinSystemRequirement()
             {
                 SRMId = 4,
                 OpSystem = "Windows 10 64 Bit (latest update)",
                 Processor = "Intel® Core™ i3-6100 / Core™ i5-2500K or AMD Ryzen™ 3 1200",
                 Memory = "8 GB RAM",
                 Graphics = "NVIDIA® GeForce® GTX 960 or AMD Radeon™ RX 470",
                 Storage = " 125 GB available space",
                 AdditionalNotes = "Broadband internet connection",
                 GameID = 4,
                 SoundCard = "Direct X 12 Compatible"
             });
            modelBuilder.Entity<RecommendSystemRequirement>().HasData(
                new RecommendSystemRequirement()
                {
                    SRRId = 1,
                    OpSystem = "Windows 10 64 Bit, Windows 8.1 64 Bit, Windows 8 64 Bit, Windows 7 64 Bit Service Pack 1",
                    Processor = "Intel Core i5 3470 @ 3.2GHz (4 CPUs) / AMD X8 FX-8350 @ 4GHz (8 CPUs)",
                    Memory = "8 GB RAM",
                    Graphics = "NVIDIA GTX 660 2GB / AMD HD 7870 2GB",
                    Storage = "72 GB available space",
                    AdditionalNotes = "",
                    GameId = 1,
                    SoundCard = "100% DirectX 10 compatible"
                });
            modelBuilder.Entity<RecommendSystemRequirement>().HasData(
               new RecommendSystemRequirement()
               {
                   SRRId = 2,
                   OpSystem = "Windows 10 April 2018 Update (v1803)",
                   Processor = "Intel Core™ i7-4770K / AMD Ryzen 5 1500X",
                   Memory = "12 GB RAM",
                   Graphics = "Nvidia GeForce GTX 1060 6GB / AMD Radeon RX 480 4GB",
                   Storage = "150 GB available space",
                   AdditionalNotes = "",
                   GameId = 2,
                   SoundCard = "Direct X Compatible"
               });
            modelBuilder.Entity<RecommendSystemRequirement>().HasData(
              new RecommendSystemRequirement()
              {
                  SRRId = 3,
                  OpSystem = "Windows 10 - 64-Bit",
                  Processor = "Intel i5-3550 @ 3.40GHz or AMD FX 8150 @ 3.6GHz",
                  Memory = "8 GB RAM",
                  Graphics = "NVIDIA GeForce GTX 670 or AMD Radeon R9 270X",
                  Storage = "50 GB available space",
                  AdditionalNotes = "Broadband internet connection",
                  GameId = 3,
                  SoundCard = "Direct X Compatible"
              });
            modelBuilder.Entity<RecommendSystemRequirement>().HasData(
              new RecommendSystemRequirement()
              {
                  SRRId = 4,
                  OpSystem = "Windows 10 64 Bit (latest update) or Windows 11 64 Bit (latest update)",
                  Processor = "Intel Core i5-6600K / Core i7-4770 or AMD Ryzen 5 1400",
                  Memory = "12 GB RAM",
                  Graphics = " NVIDIA GeForce GTX 1060 or AMD Radeon RX 580",
                  Storage = "125 GB available space",
                  AdditionalNotes = "Broadband internet connection",
                  GameId = 4,
                  SoundCard = "Direct X 12 Compatible"
              });
            var roleId = new Guid("898a5cbc-f3d9-4881-8a71-e4c97b080000");
            var adminId = new Guid("1e0241b0-cad2-4ea6-a11b-a1010a9a66e6");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role"
            });
            var roleId2 = new Guid("4ddf6713-6fa5-43b0-bea9-d0cf9e14844c");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId2,
                Name = "User",
                NormalizedName = "USER",
                Description = "User role"
            });
            var hasher = new PasswordHasher<AppUser>();

            modelBuilder.Entity<AvatarUser>().HasData(new AvatarUser()
            {
                ImageId = 1,
                ImagePath = "imgnotfound.jpg",
                UserId = adminId
            });
            modelBuilder.Entity<ThumbnailUser>().HasData(new ThumbnailUser()
            {
                ImageId = 1,
                ImagePath = "imgnotfound.jpg",
                UserId = adminId
            });
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "VesUnu",
                NormalizedUserName = "VESUNU",
                Email = "vesi_valq@abv.bg",
                NormalizedEmail = "VESI_VALQ@abv.bg",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null!, "veselin9"),
                SecurityStamp = string.Empty,
                FirstName = "Veselin",
                LastName = "Uzuntonev",
                BirthDate = new DateTime(1995, 10, 03)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
