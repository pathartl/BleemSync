using BleemSync.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Services.Abstractions
{
    public interface IGameManagerService
    {
        void AddGame(GameManagerNode node);
        void UpdateGame(GameManagerNode node);
        void UpdateGames(IEnumerable<GameManagerNode> nodes);
        void DeleteGame(GameManagerNode node);
    }
}
