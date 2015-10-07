using System.Collections.Generic;
using System.Linq;

namespace IVRRecording.Web.Models.Repository
{
    public interface IAgentRepository
    {
        IEnumerable<Agent> All();
        Agent FindByExtension(string extension);
    }
    public class AgentRepository : IAgentRepository
    {
        private readonly IVRRecordingContext _context;

        public AgentRepository()
        {
            _context = new IVRRecordingContext();
        }

        public IEnumerable<Agent> All()
        {
            return _context.Agents.ToList();
        }

        public Agent FindByExtension(string extension)
        {
            return _context.Agents.FirstOrDefault(agent => agent.Extension == extension);
        }
    }
}