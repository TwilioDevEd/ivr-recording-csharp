using System.Collections.Generic;
using System.Web.Mvc;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;
using Twilio.TwiML;

namespace IVRRecording.Web.Controllers
{
    public class ExtensionController : Controller
    {
        private readonly IAgentRepository _repository;

        public ExtensionController() : this(new AgentRepository()) {}

        public ExtensionController(IAgentRepository repository)
        {
            _repository = repository;
        }

        // POST: Extension/Connect
        [HttpPost]
        public ActionResult Connect(string digits)
        {
            var extension = digits;
            var agent = FindAgentByExtension(extension);
            if (agent == null)
            {
                return RedirectToMenu();
            }

            var response = new VoiceResponse();

            response.Say("You'll be connected shortly to your planet.",
                voice: "alice", language: "en-GB" );

            var dial = new Dial(action: Url.Action("Call", "Agent", new { agentId = agent.Id }));
            dial.Number(agent.PhoneNumber, url: Url.Action("ScreenCall", "Agent"));
            response.Dial(dial);

            return Content(response.ToString(), "application/xml");
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

        private ActionResult RedirectToMenu()
        {
            var response = new VoiceResponse();
            response.Redirect(Url.Action("Welcome", "IVR"));

            return Content(response.ToString(), "application/xml");
        }
    }
}
