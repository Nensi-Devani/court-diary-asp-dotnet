using System.Data.Entity;

namespace CourtDiary.Models
{
    public class CourtDiaryContext : DbContext
    {
        public CourtDiaryContext() : base("CourtDiaryConnection")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}