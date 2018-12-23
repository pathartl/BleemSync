using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Data.Models
{
    // This is actually probably unused by the UI, but we need it on creation of the database
    [Table("LANGUAGE_SPECIFIC")]
    public class Language
    {
        [Column("DEFAULT_VALUE")]
        public string DefaultValue { get; set; }

        [Column("LANGUAGE_ID")]
        public string LanguageId { get; set; }

        [Column("VALUE")]
        public string Value { get; set; }
    }
}
