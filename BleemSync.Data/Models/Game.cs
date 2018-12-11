using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Data.Models
{
    [Table("GAME")]
    public class Game
    {
        [Key, Column("GAME_ID")]
        public int Id { get; set; }

        [Column("GAME_TITLE_STRING")]
        public string Title { get; set; }

        [Column("PUBLISHER_NAME")]
        public string Publisher { get; set; }

        [Column("RELEASE_YEAR")]
        public int Year { get; set; }

        [Column("PLAYERS")]
        public int Players { get; set; }

        [Column("RATING_IMAGE")]
        public string RatingImage { get; set; }

        [Column("GAME_MANUAL_QR_IMAGE")]
        public string GameManualQrImage { get; set; }

        [Column("LINK_GAME_ID")]
        public string LinkGameId { get; set; }

        public ICollection<Disc> Discs { get; set; }

        public Game()
        {
            RatingImage = "CERO_A";
            GameManualQrImage = "QR_Code_GM";
            LinkGameId = "";
        }
    }
}
