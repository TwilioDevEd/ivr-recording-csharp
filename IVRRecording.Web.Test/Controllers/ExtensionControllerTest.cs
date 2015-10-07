using IVRRecording.Web.Controllers;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;
using Moq;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace IVRRecording.Web.Test.Controllers
{
    public class ExtensionControllerTest : ControllerTest
    {
        [Test]
        public void GivenAConnectAction_WhenAnAgentIsFound_ThenRespondsByDialingTheAgentsPhoneNumber()
        {
            var mockRepository = new Mock<IAgentRepository>();
            mockRepository.Setup(r => r.FindByExtension(It.IsAny<string>()))
                .Returns(new Agent {Id = 1, PhoneNumber = "+12025550142"});
            var controller = new ExtensionController(mockRepository.Object) {Url = Url};
            var result = controller.Connect("2");

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Dial").Attributes["action"].Value,
                Is.EqualTo("/Agent/Call?agentId=1"));

            Assert.That(document.SelectSingleNode("Response/Dial/Number").Attributes["url"].Value,
                Is.EqualTo("/Agent/ScreenCall"));
            Assert.That(document.SelectSingleNode("Response/Dial/Number").InnerText,
                Is.EqualTo("+12025550142"));
        }

        [Test]
        public void GivenAConnectAction_WhenAnAgentIsNotFound_ThenRespondsRedirectingToMainMenu()
        {
            var mockRepository = new Mock<IAgentRepository>();
            mockRepository.Setup(r => r.FindByExtension(It.IsAny<string>()));
            var controller = new ExtensionController(mockRepository.Object) {Url = Url};
            var result = controller.Connect("*");

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Redirect").InnerText,
                Is.EqualTo("/IVR/Welcome"));
        }
    }
}
