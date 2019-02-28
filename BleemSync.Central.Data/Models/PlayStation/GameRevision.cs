using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Central.Data.Models.PlayStation
{
    public class GameRevision : Revision
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        public int RevisedGameId { get; set; }
        public virtual Game RevisedGame { get; set; }
    }
}
