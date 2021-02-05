using System;
using System.Web.Mvc;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace IVRRecording.Web.Controllers
{
    public class IVRController : TwilioController
    {
        //POST: IVR/Welcome
        [HttpPost]
        public TwiMLResult Welcome()
        {
            var response = new VoiceResponse();
            var gather = new Gather(action: new Uri(Url.Action("Show", "Menu"), UriKind.Relative), numDigits: 1);
            gather.Play(new Uri("https://can-tasty-8188.twil.io/assets/et-phone.mp3"), loop: 3);
            response.Append(gather);

            return TwiML(response);
        }
    }
}