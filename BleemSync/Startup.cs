using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using BleemSync.Services;
using BleemSync.Services.Extensions;
using BleemSync.Services.ViewModels;
using System.Data.SqlClient;
using System.IO;
using BleemSync.Data;
using Microsoft.EntityFrameworkCore;

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

            Directory.CreateDirectory(Path.Combine(Configuration["BleemSync:Destination"], Configuration["BleemSync:Path"]));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExtCore(ExtensionsPath, Configuration["Extensions:IncludingSubpaths"].ToLower() == true.ToString());

            services.AddDbContext<DatabaseContext>();

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1474560000;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddScoped<UsbService>();
            services.AddScoped<GameManagerFileService>();
            services.AddScoped<GameManagerNodeService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.ConfigureWritable<BleemSyncConfiguration>(Configuration.GetSection("BleemSync"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                context.Database.Migrate();
            }

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
    }
}
