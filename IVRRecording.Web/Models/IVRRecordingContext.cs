using System.Data.Entity;

namespace IVRRecording.Web.Models
{
    public class IVRRecordingContext : DbContext
    {
        public IVRRecordingContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Recording> Recordings { get; set; } 
    }
}