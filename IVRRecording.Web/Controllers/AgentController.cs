using System;
using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class AgentController : TwilioController
    {
        // GET: Agent/Call
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
                action = "/Agent/Hangup",
                transcribeCallback = string.Format("/Recording/Create?agentId={0}", agentId)
            });

            response.Say("No record received. Goodbye",
                new { voice = "alice", language = "en-GB" });

            response.Hangup();

            return new TwiMLResult(response);
        }

        // GET: Agent/ScreenCall
        public TwiMLResult ScreenCall(string from)
        {
            var response = new TwilioResponse();
            var incomingCallMessage = "You have an incoming call from: " +
                                      SpelledPhoneNumber(from);
            response.BeginGather(new {numDigits = 1, action = "/Agent/ConnectMessage"})
                .Say(incomingCallMessage)
                .Say("Press any key to accept")
                .Say("Sorry. Did not get your response")
                .EndGather();
            response.Hangup();

            return new TwiMLResult(response);
        }

        // GET: Agent/ConnectMessage
        public TwiMLResult ConnectMessage()
        {
            return new TwiMLResult(new TwilioResponse()
                .Say("Connecting you to the extraterrestrial in distress"));
        }

        // GET: Agent/Hangup
        public TwiMLResult Hangup()
        {
            var response = new TwilioResponse();
            response.Say("Thanks for your message. Goodbye",
                new {voice = "alice", language = "en-GB"});
            response.Hangup();

            return new TwiMLResult(response);
        }

        private static string SpelledPhoneNumber(string phoneNumber)
        {
           return string.Join(", ", phoneNumber.ToCharArray());
        }
    }
}