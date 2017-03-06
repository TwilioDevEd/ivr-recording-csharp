using System.Data.Entity.Migrations;
using IVRRecording.Web.Models;

namespace IVRRecording.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<IVRRecordingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IVRRecordingContext context)
        {
            context.Agents.AddOrUpdate(
                agent => new {agent.Extension, agent.PhoneNumber},
                new Agent {Extension = "Brodo", PhoneNumber = "+15552483591"},
                new Agent {Extension = "Dagobah", PhoneNumber = "15558675309"},
                new Agent {Extension = "Oober", PhoneNumber = "+15553185602"});

            context.SaveChanges();
        }
    }
}
