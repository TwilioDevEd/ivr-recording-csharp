using System.Collections.Generic;
using IVRRecording.Web.Models;
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
            var agent = FindAgentByExtension(extension);
            if (agent == null)
            {
                return RedirectToMenu();
            }

            var response = new TwilioResponse();

            response.Say("You'll be connected shortly to your planet.",
                new { voice = "alice", language = "en-GB" });

            var number = new Number(agent.PhoneNumber, new { url = "/Agent/ScreenCall" });
            response.Dial(number, new {action = string.Format("/Agent/Call?agentId={0}", agent.Id)});

            return new TwiMLResult(response);
        }

        private Agent FindAgentByExtension(string extension)
        {
            var planetExtensions = new Dictionary<string, string>
            {
                {"2", "Brodo"},
                {"3", "Dagobah"},
                {"4", "Oober"}
            };

            string agentExtension;
            planetExtensions.TryGetValue(extension, out agentExtension);
            return _repository.FindByExtension(agentExtension);
        }
        private static TwiMLResult RedirectToMenu()
        {
            var response = new TwilioResponse();
            response.Redirect("/IVR/Welcome");

            return new TwiMLResult(response);
        }
    }
}