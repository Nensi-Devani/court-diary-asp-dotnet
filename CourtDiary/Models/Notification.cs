using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Notification
    {
        [Key]
        public int notification_id { get; set; }

        public int user_id { get; set; }

        public string title { get; set; }

        public string message { get; set; }

        public bool is_read { get; set; } = false;

        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
