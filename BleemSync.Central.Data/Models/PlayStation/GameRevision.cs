using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.PlayStation
{
    public class GameRevision : Revision
    {
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }

        public int RevisedGameId { get; set; }
        [ForeignKey("RevisedGameId")]
        public virtual Game RevisedGame { get; set; }
    }
}
