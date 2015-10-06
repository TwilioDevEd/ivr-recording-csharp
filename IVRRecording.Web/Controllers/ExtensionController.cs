using IVRRecording.Web.Models.Repository;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class ExtensionController : TwilioController
    {
        private readonly IAgentRepository _repository;

        public ExtensionController() : this(new AgentRepository()) {}

        public ExtensionController(IAgentRepository repository)
        {
            _repository = repository;
        }

        // GET: Extension/Connect
        public TwiMLResult Connect(string digits)
        {
            var extension = digits;
            var agent = _repository.FindByExtension(extension);
            var response = new TwilioResponse();

            response.Say("You'll be connected shortly to your planet.",
                new { voice = "alice", language = "en-GB" });

            var number = new Number(agent.PhoneNumber, new { url = "screen/call" });
            response.Dial(number, new { action = "" });

            return new TwiMLResult(response);
        }
    }
}