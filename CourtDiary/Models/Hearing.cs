using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Hearing
    {
        [Key]
        public int hearing_id { get; set; }

        public int case_id { get; set; }

        public DateTime? hearing_date { get; set; }

        public DateTime? next_hearing_date { get; set; }

        public string notes { get; set; }

        public string status { get; set; }

        public decimal? fee_amount { get; set; }

        public string payment_status { get; set; }

        public DateTime? payment_date { get; set; }

        public string payment_method { get; set; }
    }
}
