using BleemSync.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Services
{
    public abstract class DataService : IDisposable
    {
        protected DatabaseContext DatabaseContext { get; set; }

        public DataService()
        {
            DatabaseContext = new DatabaseContext();
            DatabaseContext.Database.EnsureCreated();
            DatabaseContext.Database.Migrate();
        }

        public void Dispose()
        {
            DatabaseContext.Dispose();
        }
    }
}
