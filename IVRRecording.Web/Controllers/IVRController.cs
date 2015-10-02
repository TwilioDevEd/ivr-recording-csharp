using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class IVRController : TwilioController
    {
        [ActionName("Welcome")]
        public TwiMLResult ShowWelcome()
        {
            var response = new TwilioResponse();
            response.BeginGather(new {action = "", numDigits = 1})
                .Play("http://howtodocs.s3.amazonaws.com/et-phone.mp3", new { loop = 3 })
                .EndGather();

            return TwiML(response);
        }
    }
}