using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Client
    {
        [Key]   // 🔥 THIS FIXES YOUR ERROR
        public int client_id { get; set; }

        public int user_id { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string avatar { get; set; }

        public string? ImagePath { get; set; }

        public string address { get; set; }

        public string description { get; set; }

        public DateTime created_at { get; set; }


    }
}

