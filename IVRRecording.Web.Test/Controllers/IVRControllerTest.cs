using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IVRRecording.Web.Controllers;
using Moq;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace IVRRecording.Web.Test.Controllers
{
    class IVRControllerTest
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
        public void GivenAWelcomeAction_ThenTheResponseContainsGatherPlay()
        {
            var controller = new IVRController();
            var result = controller.Welcome();

            result.ExecuteResult(_mockControllerContext.Object);

            var document = TestHelper.LoadXml(_result.ToString());

            Assert.That(document.SelectSingleNode("Response/Gather/Play"), Is.Not.Null);
            Assert.That(document.SelectSingleNode("Response/Gather").Attributes["action"].Value, Is.Empty);
        }
    }
}
