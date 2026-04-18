using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Case_Party
    {
        [Key]
        public int party_id { get; set; }

        public int case_id { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("case_id")]
        public virtual Case Case { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string notes { get; set; }

        public string avatar { get; set; }
    }
}
