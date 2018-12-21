using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BleemSync.Extensions.Infrastructure.ViewModels;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using BleemSync.Extensions.Infrastructure.Attributes;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;

namespace BleemSync.Services
{
    public class MenuService
    {
        IUrlHelper Url { get; set; }

        public MenuService(IUrlHelper urlHelper)
        {
            Url = urlHelper;
        }

        public List<MenuItem> GetMenuItems()
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
                            Position = sectionAttribute.Position,
                        };

                        // Loop through the controllers and add public actions as menu items if
                        // the MenuItemAttribute is set
                        foreach (var action in controller.GetMethods().Where(m => m.IsPublic && m.IsDefined(typeof(MenuItemAttribute)))) {
                            var menuItemAttribute = action.GetCustomAttribute<MenuItemAttribute>();

                            var child = new MenuItem()
                            {
                                Name = menuItemAttribute.Name,
                                Position = menuItemAttribute.Position
                            };

                            if (areaAttribute != null)
                            {
                                child.Url = Url.Action(action.Name, controller.Name.Replace("Controller", ""), new { Area = areaAttribute.RouteValue });
                            }
                            else
                            {
                                child.Url = Url.Action(action.Name, controller.Name.Replace("Controller", ""));
                            }

                            sectionMenuItem.Children.Add(child);
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

            return menuItems;
        }
    }
}
