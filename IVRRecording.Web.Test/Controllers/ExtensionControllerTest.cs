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
    public class ExtensionControllerTest
    {
        private StringBuilder _result;
        private Mock<ControllerContext> _mockControllerContext;

        [SetUp]
        public void Setup()
        {
            _result = new StringBuilder();
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(s => s.Write(It.IsAny<string>())).Callback<string>(c => _result.Append(c));
            mockResponse.Setup(s => s.Output).Returns(new StringWriter(_result));

            _mockControllerContext = new Mock<ControllerContext>();
            _mockControllerContext.Setup(x => x.HttpContext.Response).Returns(mockResponse.Object);
        }

        [Test]
        public void GivenAConnectAction_WhenAnAgentIsFound_ThenRespondsByDialingTheAgentsPhoneNumber()
        {
            var mockRepository = new Mock<IAgentRepository>();
            mockRepository.Setup(r => r.FindByExtension(It.IsAny<string>()))
                .Returns(new Agent {PhoneNumber = "+12025550142"});
            var controller = new ExtensionController(mockRepository.Object);
            var result = controller.Connect("agent-extension");

            result.ExecuteResult(_mockControllerContext.Object);

            var document = TestHelper.LoadXml(_result.ToString());

            Assert.That(document.SelectSingleNode("Response/Dial/Number"), Is.Not.Null);
            Assert.That(document.SelectSingleNode("Response/Dial/Number").InnerText,
                Is.EqualTo("+12025550142"));

        }
    }
}
