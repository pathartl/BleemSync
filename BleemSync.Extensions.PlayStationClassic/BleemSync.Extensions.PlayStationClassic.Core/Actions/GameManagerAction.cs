using BleemSync.Extensions.PlayStationClassic.Core.Services;
using BleemSync.Services.Abstractions;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Actions
{
    public class GameManagerAction : IConfigureServicesAction
    {
        public int Priority => 1000;

        public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            services.AddScoped(typeof(IGameManagerService), typeof(GameManagerService));
        }
    }
}