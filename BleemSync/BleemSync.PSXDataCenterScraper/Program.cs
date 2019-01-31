﻿using System.IO;
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
                options.UseMySql(configuration["MySQLConnectionString"])
            );

            var serviceProvider = services.BuildServiceProvider();

            var _context = serviceProvider.GetService<DatabaseContext>();

            var scraper = new Scraper(_context);
            scraper.ScrapeMainList("https://psxdatacenter.com/ulist.html");
            scraper.ScrapeMainList("https://psxdatacenter.com/plist.html");
            scraper.ScrapeMainList("https://psxdatacenter.com/jlist.html");
            scraper.ScrapeMainList("https://psxdatacenter.com/psp/ntsc-j_list.html");
            scraper.ScrapeMainList("https://psxdatacenter.com/psp/ntsc_ulist.html");
            scraper.ScrapeMainList("https://psxdatacenter.com/psp/pal_list.html");
           
            
        }
    }
}
