using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace IVRRecording.Web.Controllers
{
    public class IVRController : TwilioController
    {
        //POST: IVR/Welcome
        [HttpPost]
        public TwiMLResult Welcome()
        {
            var response = new TwilioResponse();
            response.BeginGather(new {action = Url.Action("Show", "Menu"), numDigits = 1})
                .Play("http://howtodocs.s3.amazonaws.com/et-phone.mp3", new { loop = 3 })
                .EndGather();

            return TwiML(response);
        }
    }
}