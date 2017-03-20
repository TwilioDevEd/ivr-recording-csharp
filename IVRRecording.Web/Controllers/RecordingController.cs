using System;
using System.Web.Mvc;
using System.Xml.Linq;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;
using Twilio.AspNet.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class RecordingController : TwilioController
    {
        private readonly IRecordingRepository _repository;

        public RecordingController() : this(new RecordingRepository()) {}

        public RecordingController(IRecordingRepository repository)
        {
            _repository = repository;
        }

        // POST: Recording/Create
        [HttpPost]
        public TwiMLResult Create(string agentId, string caller, string transcriptionText, string recordingUrl)
        {

            _repository.Create(
                new Recording
                {
                    AgentId = Convert.ToInt32(agentId),
                    PhoneNumber = caller,
                    Transcription = transcriptionText,
                    Url = recordingUrl
                });

            var response = new XDocument(new XElement("Root", "Recording saved"));

            return new TwiMLResult(response);
        }
    }
}