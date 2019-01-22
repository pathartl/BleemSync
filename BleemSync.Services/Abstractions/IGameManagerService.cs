using BleemSync.Data.Entities;
using System.Collections.Generic;

namespace BleemSync.Services.Abstractions
{
    public interface IGameManagerService
    {
        void AddGame(GameManagerNode node);
        void UpdateGame(GameManagerNode node);
        void UpdateGames(IEnumerable<GameManagerNode> nodes);
        void DeleteGame(GameManagerNode node);
        IEnumerable<GameManagerNode> GetGames();
        void Sync();
    }
}
