using System;
using System.Web.Mvc;
using IVRRecording.Web.Models.Repository;
using Twilio.TwiML;

namespace IVRRecording.Web.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentRepository _repository;

        public AgentController() : this(new AgentRepository()) {}

        public AgentController(IAgentRepository repository)
        {
            _repository = repository;
        }

        // GET: Agent
        public ActionResult Index()
        {
            var agents = _repository.All();
            return View(agents);
        }

        // POST: Agent/Call
        [HttpPost]
        public ActionResult Call(string agentId, string dialCallStatus)
        {
            if (dialCallStatus == "completed") return Content(String.Empty);

            var response = new VoiceResponse();
            response.Say(
                body: "It appears that no agent is available. " +
                         "Please leave a message after the beep",
                language: "en-GB",
                voice: "alice"
            );

            response.Record(
                maxLength: 20,
                playBeep: true,
                action: Url.Action("Hangup"),
                transcribeCallback: Url.Action("Create", "Recording", new {agentId})
            );

            response.Say(
                body: "No record received. Goodbye",
                language: "en-GB",
                voice: "alice"
            );

            response.Hangup();

            return Content(response.ToString(), "application/xml");
        }

        // POST: Agent/ScreenCall
        [HttpPost]
        public ActionResult ScreenCall(string from)
        {
            var response = new VoiceResponse();

            var incomingCallMessage = "You have an incoming call from: " +
                                      SpelledPhoneNumber(from);
            var gather = new Gather(
                numDigits: 1,
                action: Url.Action("ConnectMessage")
            );
            gather.Say(incomingCallMessage)
                  .Say("Press any key to accept");
            response.Gather(gather);

            response.Say("Sorry. Did not get your response");
            response.Hangup();

            return Content(response.ToString(), "application/xml");
        }

        // GET: Agent/ConnectMessage
        public ActionResult ConnectMessage()
        {
            var response = new VoiceResponse()
                .Say("Connecting you to the extraterrestrial in distress");
            return Content(response.ToString(), "application/xml");
        }

        // POST: Agent/Hangup
        [HttpPost]
        public ActionResult Hangup()
        {
            var response = new VoiceResponse();
            response.Say(
                body: "Thanks for your message. Goodbye",
                language: "en-GB",
                voice: "alice"
            );
            response.Hangup();

            return Content(response.ToString(), "application/xml");
        }

        private static string SpelledPhoneNumber(string phoneNumber)
        {
           return string.Join(", ", phoneNumber.ToCharArray());
        }
    }
}