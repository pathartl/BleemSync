using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using BleemSync.Data;
using System.Reflection;
using BleemSync.Services;

namespace BleemSync
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string ExtensionsPath;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            ExtensionsPath = hostingEnvironment.ContentRootPath + Configuration["Extensions:Path"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExtCore(ExtensionsPath, Configuration["Extensions:IncludingSubpaths"].ToLower() == true.ToString());
            services.Configure<StorageContextOptions>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("Default");
                options.MigrationsAssembly = typeof(DesignTimeStorageContextFactory).GetTypeInfo().Assembly.FullName;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1474560000;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddScoped<UsbService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            var sp = services.BuildServiceProvider();
            DesignTimeStorageContextFactory.Initialize(sp);
            DesignTimeStorageContextFactory.StorageContext.Database.Migrate();

            // Import games from older version
            ImportInitialGames(sp);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseExtCore();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        void ImportInitialGames(ServiceProvider sp)
        {
            var dbContext = DesignTimeStorageContextFactory.StorageContext;
            if (dbContext == null) return;
            var deviceGameManager = sp.GetService<BleemSync.Services.Abstractions.IGameManagerService>();
            if (deviceGameManager == null) return;
            var countTask = dbContext.Set<BleemSync.Data.Entities.GameManagerNode>().CountAsync();
            countTask.Wait();
            if (countTask.Result == 0)
            {
                dbContext.AddRange(deviceGameManager.GetGames());
                dbContext.SaveChanges();
            }
        }
    }
}
