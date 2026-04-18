using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourtDiary.Models
{
    public enum UploadCategory
    {
        Court = 1,
        Evidence = 2,
        Client = 3
    }

    public class Upload
    {
        [Key]
        public int upload_id { get; set; }

        [Required]
        public int case_id { get; set; }

        [ForeignKey("case_id")]
        public Case? Case { get; set; }   // ✅ FIXED

        [Required]
        public string? upload_url { get; set; }   // ✅ SAFE

        [Required]
        public UploadCategory upload_category { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;
    }
}