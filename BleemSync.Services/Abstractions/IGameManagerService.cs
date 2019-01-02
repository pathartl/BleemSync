using BleemSync.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Services.Abstractions
{
    public interface IGameManagerService
    {
        void UploadGame(GameManagerNode node, IEnumerable<string> files);
    }
}
