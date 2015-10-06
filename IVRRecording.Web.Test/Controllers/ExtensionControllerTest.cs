using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
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
                .Returns(new Agent {PhoneNumber = "+12025550142"});
            var controller = new ExtensionController(mockRepository.Object);
            var result = controller.Connect("agent-extension");

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Dial/Number"), Is.Not.Null);
            Assert.That(document.SelectSingleNode("Response/Dial/Number").InnerText,
                Is.EqualTo("+12025550142"));

        }
    }
}
