using System;
using System.Web.Mvc;
using IVRRecording.Web.Models.Repository;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class AgentController : TwilioController
    {
        private readonly IAgentRepository _repository;

        public AgentController() : this(new AgentRepository()) {}

        public AgentController(IAgentRepository repository)
        {
            _repository = repository;
        }

        // GET: Recording
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

            var response = new TwilioResponse();
            response.Say("It appears that no agent is available. " +
                         "Please leave a message after the beep",
                new { voice = "alice", language = "en-GB" });

            response.Record(new
            {
                maxLength = "20",
                action = Url.Action("Hangup"),
                transcribeCallback = Url.Action("Create", "Recording", new {agentId})
            });

            response.Say("No record received. Goodbye",
                new { voice = "alice", language = "en-GB" });

            response.Hangup();

            return TwiML(response);
        }

        // POST: Agent/ScreenCall
        [HttpPost]
        public TwiMLResult ScreenCall(string from)
        {
            var response = new TwilioResponse();
            var incomingCallMessage = "You have an incoming call from: " +
                                      SpelledPhoneNumber(from);
            response.BeginGather(new {numDigits = 1, action = Url.Action("ConnectMessage")})
                .Say(incomingCallMessage)
                .Say("Press any key to accept")
                .EndGather();

            response.Say("Sorry. Did not get your response");
            response.Hangup();

            return TwiML(response);
        }

        // GET: Agent/ConnectMessage
        public TwiMLResult ConnectMessage()
        {
            return TwiML(new TwilioResponse()
                .Say("Connecting you to the extraterrestrial in distress"));
        }

        // POST: Agent/Hangup
        [HttpPost]
        public TwiMLResult Hangup()
        {
            var response = new TwilioResponse();
            response.Say("Thanks for your message. Goodbye",
                new {voice = "alice", language = "en-GB"});
            response.Hangup();

            return TwiML(response);
        }

        private static string SpelledPhoneNumber(string phoneNumber)
        {
           return string.Join(", ", phoneNumber.ToCharArray());
        }
    }
}