using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Data.EF
{
    public class GameShopStoreVTwoDBContextFactory : IDesignTimeDbContextFactory<GameShopStoreVTwoDBContext>
    {
        public GameShopStoreVTwoDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
              .Build();
            var connectionstring = configuration.GetConnectionString("GameShopStoreDatabase");
            var optionsBuilder = new DbContextOptionsBuilder<GameShopStoreVTwoDBContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            return new GameShopStoreVTwoDBContext(optionsBuilder.Options);
        }
    }
}
