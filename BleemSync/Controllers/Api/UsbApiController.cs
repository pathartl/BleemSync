using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BleemSync.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BleemSync.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsbApiController : ControllerBase
    {
        private readonly UsbService _usbService;

        public UsbApiController(UsbService usbService)
        {
            _usbService = usbService;
        }

        public void SwitchDestinationDevice([FromForm] string drive)
        {
            _usbService.SetCurrentDrive(drive);
        }
    }
}