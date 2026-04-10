using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        public bool is_varified { get; set; } = false;

        public string avatar { get; set; }

        public string office_email { get; set; }

        public string office_address { get; set; }

        public string office_phone_no { get; set; }

        public int role { get; set; } = 0;

        public DateTime created_at { get; set; } = DateTime.Now;
    }
}