using System.Collections.Generic;
using System.Linq;

namespace IVRRecording.Web.Models.Repository
{
    public interface IRecordingRepository
    {
        IEnumerable<Recording> All();
        void Create(Recording recording);
    }

    public class RecordingRepository : IRecordingRepository
    {
        private readonly IVRRecordingContext _context;

        public RecordingRepository()
        {
            _context = new IVRRecordingContext();
        }

        public IEnumerable<Recording> All()
        {
            return _context.Recordings.ToList();
        }

        public void Create(Recording recording)
        {
            _context.Recordings.Add(recording);
            _context.SaveChanges();
        }
    }
}