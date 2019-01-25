using System;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace BleemSync.Extensions.PlayStationClassic.Core.Actions
{
    public class UseMvcAction : IUseMvcAction
    {
        public int Priority => 1000;
        public void Execute(IRouteBuilder routeBuilder, IServiceProvider serviceProvider)
        {
            routeBuilder.MapRoute(name: "PlayStationClassic", template: "{controller}/{action}", defaults: new { controller = "PlayStationClassic", action = "Index" });
        }
    }
}
