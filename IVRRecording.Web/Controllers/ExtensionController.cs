using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

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

        // POST: Extension/Connect
        [HttpPost]
        public TwiMLResult Connect(string digits)
        {
            var extension = digits;
            var agent = FindAgentByExtension(extension);
            if (agent == null)
            {
                return RedirectToMenu();
            }

            var response = new VoiceResponse();

            response.Say("You'll be connected shortly to your planet.",
                voice: Say.VoiceEnum.PollyAmy, language: "en-GB" );

            var dial = new Dial(action: new Uri(Url.Action("Call", "Agent", new { agentId = agent.Id }), UriKind.Relative));
            dial.Number(agent.PhoneNumber, url: new Uri(Url.Action("ScreenCall", "Agent"), UriKind.Relative));
            response.Append(dial);

            return TwiML(response);
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

        private TwiMLResult RedirectToMenu()
        {
            var response = new VoiceResponse();
            response.Redirect(new Uri(Url.Action("Welcome", "IVR"), UriKind.Relative));

            return TwiML(response);
        }
    }
}
