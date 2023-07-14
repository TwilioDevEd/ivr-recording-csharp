using System;
using System.Web.Mvc;
using IVRRecording.Web.Models.Repository;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using System.Xml.Linq;

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

        // GET: Agent
        public ActionResult Index()
        {
            var agents = _repository.All();
            return View(agents);
        }

        // POST: Agent/Call
        [HttpPost]
        public TwiMLResult Call(string agentId, string dialCallStatus)
        {
            if (dialCallStatus == "completed")
            {
                var emptyResponse = new XDocument(new XElement("Root", ""));
                return new TwiMLResult(emptyResponse);
            }

            var response = new VoiceResponse();
            response.Say(
                "It appears that no agent is available. Please leave a message after the beep",
                language: "en-GB",
                voice: Say.VoiceEnum.PollyAmy
            );

            response.Record(
                maxLength: 20,
                playBeep: true,
                action: new Uri(Url.Action("Hangup"), UriKind.Relative),
                transcribeCallback: new Uri(Url.Action("Create", "Recording", new {agentId}), UriKind.Relative)
            );

            response.Say(
                "No record received. Goodbye",
                language: "en-GB",
                voice: Say.VoiceEnum.PollyAmy
            );

            response.Hangup();

            return TwiML(response);
        }

        // POST: Agent/ScreenCall
        [HttpPost]
        public TwiMLResult ScreenCall(string from)
        {
            var response = new VoiceResponse();

            var incomingCallMessage = "You have an incoming call from: " +
                                      SpelledPhoneNumber(from);
            var gather = new Gather(
                numDigits: 1,
                action: new Uri(Url.Action("ConnectMessage"), UriKind.Relative)
            );
            gather.Say(incomingCallMessage)
                  .Say("Press any key to accept");
            response.Append(gather);

            response.Say("Sorry. Did not get your response");
            response.Hangup();

            return TwiML(response);
        }

        // GET: Agent/ConnectMessage
        public TwiMLResult ConnectMessage()
        {
            var response = new VoiceResponse()
                .Say("Connecting you to the extraterrestrial in distress");
            return TwiML(response);
        }

        // POST: Agent/Hangup
        [HttpPost]
        public TwiMLResult Hangup()
        {
            var response = new VoiceResponse();
            response.Say(
                "Thanks for your message. Goodbye",
                language: "en-GB",
                voice: Say.VoiceEnum.PollyAmy
            );
            response.Hangup();

            return TwiML(response);
        }

        private static string SpelledPhoneNumber(string phoneNumber)
        {
           return string.Join(", ", phoneNumber.ToCharArray());
        }
    }
}