using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.Extensions.Infrastructure.ViewModels;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BleemSync.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        public SidebarViewComponent() { }
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>();

            foreach (var assembly in ExtensionManager.Assemblies)
            {
                var controllers = assembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type));

                foreach (var controller in controllers.Where(c => c.IsDefined(typeof(MenuSectionAttribute))))
                {
                    try
                    {
                        var sectionAttribute = controller.GetCustomAttribute<MenuSectionAttribute>();
                        var areaAttribute = controller.GetCustomAttribute<AreaAttribute>();

                        var existingSectionMenuItem = menuItems.SingleOrDefault(mi => mi.Name == sectionAttribute.Name);
                        var sectionMenuItem = new MenuItem()
                        {
                            Name = sectionAttribute.Name,
                            Icon = sectionAttribute.Icon,
                            Position = sectionAttribute.Position
                        };

                        foreach (var action in controller.GetMethods().Where(m => m.IsPublic && m.IsDefined(typeof(MenuItemAttribute))))
                        {
                            var menuItemAttribute = action.GetCustomAttribute<MenuItemAttribute>();

                            var subMenuItem = new MenuItem()
                            {
                                Name = menuItemAttribute.Name,
                                Position = menuItemAttribute.Position
                            };

                            if (areaAttribute != null)
                            {
                                subMenuItem.Url = Url.Action(action.Name, controller.Name.Replace("Controller", ""), new { Area = areaAttribute.RouteValue });
                            }
                            else
                            {
                                subMenuItem.Url = Url.Action(action.Name, controller.Name.Replace("Controller", ""));
                            }

                            sectionMenuItem.Children.Add(subMenuItem);
                        }

                        if (existingSectionMenuItem != null)
                        {
                            existingSectionMenuItem.Children.AddRange(sectionMenuItem.Children);
                            sectionMenuItem = existingSectionMenuItem;
                        }
                        else
                        {
                            menuItems.Add(sectionMenuItem);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

            return View("Default", menuItems);
        }
    }
}
