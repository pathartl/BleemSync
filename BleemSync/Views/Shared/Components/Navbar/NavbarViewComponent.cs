using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.Extensions.Infrastructure.ViewModels;
using BleemSync.Services;
using BleemSync.ViewModels;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BleemSync.Views.Shared.Components.Sidebar
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly UsbService _usbService;

        public NavbarViewComponent(UsbService usbService) {
            _usbService = usbService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new NavbarViewModel() {
                Drives = _usbService.GetDrives().ToList(),
                CurrentDrive = _usbService.GetCurrentDrive()
            };

            return View("Default", viewModel);
        }
    }
}
