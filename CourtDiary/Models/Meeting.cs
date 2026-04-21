using System;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Subtitle { get; set; } // Assignee or email

        public int CommentsCount { get; set; } = 0;

        public int ViewsCount { get; set; } = 0;

        [StringLength(50)]
        public string TagText { get; set; } // e.g. URGENT, DONE, PENDING

        [StringLength(50)]
        public string TagColor { get; set; } // e.g. danger, success, warning, primary

        [Required]
        [StringLength(100)]
        public string Category { get; set; } // e.g. Team Tasks, Ideas to Vote On, Other stuff

        [Required]
        public DateTime EventDate { get; set; } = DateTime.Now;

        public string Location { get; set; }
        public string Description { get; set; }
    }
}
