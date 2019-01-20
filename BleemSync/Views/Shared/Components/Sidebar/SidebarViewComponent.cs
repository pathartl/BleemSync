using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.Extensions.Infrastructure.ViewModels;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                    catch { }
                }
            }

            return View("Default", menuItems);
        }
    }
}
