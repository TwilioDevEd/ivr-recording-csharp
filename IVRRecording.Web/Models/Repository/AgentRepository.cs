using System.Linq;

namespace IVRRecording.Web.Models.Repository
{
    public interface IAgentRepository
    {
        Agent FindByExtension(string extension);
    }
    public class AgentRepository : IAgentRepository
    {
        private readonly IVRRecordingContext _context;

        public AgentRepository()
        {
            _context = new IVRRecordingContext();
        }

        public Agent FindByExtension(string extension)
        {
            return _context.Agents.FirstOrDefault(agent => agent.Extension == extension);
        }
    }
}