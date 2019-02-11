using System.IO;
using BleemSync.Central.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BleemSync.PSXDataCenterScraper;

namespace BleemSync.Scrapers.PSXDataCenterScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddDbContext<DatabaseContext>(options =>
                options.UseMySql("Server=10.0.1.10;Port=3308;Database=psxdatacenter;User=psx;Password=psx;charset=utf8mb4")
            );

            var serviceProvider = services.BuildServiceProvider();

            var _context = serviceProvider.GetService<DatabaseContext>();

            var scraper = new PlayStationScraper(_context);
            scraper.ScrapeMainList("http://10.0.1.12/psxdatacenter/psxdatacenter.com/ulist.html");
            //scraper.ScrapeMainList("https://psxdatacenter.com/plist.html");
            //scraper.ScrapeMainList("https://psxdatacenter.com/jlist.html");
        }
    }
}
