using System.Data.Entity;

namespace CourtDiary.Models
{
    public class CourtDiaryContext : DbContext
    {
        public CourtDiaryContext() : base("CourtDiaryConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Case> Cases { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Upload> Uploads { get; set; }

        public DbSet<Case_Party> Case_Parties { get; set; }

        public DbSet<Hearing> Hearings { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        
    }
}
