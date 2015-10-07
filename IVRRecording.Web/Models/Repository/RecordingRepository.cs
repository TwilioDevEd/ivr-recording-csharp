namespace IVRRecording.Web.Models.Repository
{
    public interface IRecordingRepository
    {
        void Create(Recording recording);
    }

    public class RecordingRepository : IRecordingRepository
    {
        private readonly IVRRecordingContext _context;

        public RecordingRepository()
        {
            _context = new IVRRecordingContext();
        }

        public void Create(Recording recording)
        {
            _context.Recordings.Add(recording);
            _context.SaveChanges();
        }
    }
}