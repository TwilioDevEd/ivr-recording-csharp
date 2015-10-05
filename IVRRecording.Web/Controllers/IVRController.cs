using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class IVRController : TwilioController
    {
        public TwiMLResult Welcome()
        {
            var response = new TwilioResponse();
            response.BeginGather(new {action = "", numDigits = 1})
                .Play("http://howtodocs.s3.amazonaws.com/et-phone.mp3", new { loop = 3 })
                .EndGather();

            return TwiML(response);
        }
    }
}