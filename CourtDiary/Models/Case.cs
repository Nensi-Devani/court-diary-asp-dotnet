using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Case
    {
        [Key]
        public int case_id { get; set; }

        public int user_id { get; set; }

        public int? client_id { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("client_id")]
        public virtual Client Client { get; set; }

        public string case_no { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string previous_hearing_summary { get; set; }

        public string hearing_notes { get; set; }

        public string location { get; set; }

        public string status { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        public ICollection<Upload> Uploads { get; set; }

        public ICollection<Case_Party> Case_Parties { get; set; }

        public ICollection<Hearing> Hearings { get; set; }
    }
}