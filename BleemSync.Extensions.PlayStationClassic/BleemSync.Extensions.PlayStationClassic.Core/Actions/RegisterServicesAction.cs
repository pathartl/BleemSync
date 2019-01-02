using BleemSync.Data;
using BleemSync.Extensions.PlayStationClassic.Core.Services;
using BleemSync.Services.Abstractions;
using ExtCore.Infrastructure.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Actions
{
    public class RegisterServicesAction : IConfigureServicesAction
    {
        public int Priority => 1000;

        public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            services.AddScoped(typeof(IGameManagerService), typeof(GameManagerService));
            services.AddDbContext<MenuDatabaseContext>(options =>
                options.UseSqlite($"Data Source=ui_menu.db")
            );
        }
    }
}
