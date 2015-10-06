using System.Web.Mvc;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;

namespace IVRRecording.Web.Controllers
{
    public class RecordingController : Controller
    {
        private readonly IRecordingRepository _repository;

        public RecordingController() : this(new RecordingRepository()) {}

        public RecordingController(IRecordingRepository repository)
        {
            _repository = repository;
        }
        // GET: Recording
        public ActionResult Index()
        {
            return null;
        }

        // POST: Recording/Create
        [HttpPost]
        public ActionResult Create(int agentId, string caller, string transcriptionText, string recordingUrl)
        {

            _repository.Create(
                new Recording
                {
                    AgentId = agentId,
                    PhoneNumber = caller,
                    Transcription = transcriptionText,
                    Url = recordingUrl
                });

            return Content("Recording saved");
        }
    }
}