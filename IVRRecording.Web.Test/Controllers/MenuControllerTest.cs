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
    public class MenuControllerTest
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
        public void GivenAShowAction_WhenTheSelectedOptionIs1_ThenTheResponseContainsSayTwiceAndAHangup()
        {
            var controller = new MenuController();
            var result = controller.Show("1");

            result.ExecuteResult(_mockControllerContext.Object);

            var document = TestHelper.LoadXml(_result.ToString());

            Assert.That(document.SelectNodes("Response/Say").Count, Is.EqualTo(2));
            Assert.That(document.SelectSingleNode("Response/Hangup"), Is.Not.Null);
        }

        [Test]
        public void GivenAShowAction_WhenTheSelectedOptionIs2_ThenTheResponseContainsGatherAndSay()
        {
            var controller = new MenuController();
            var result = controller.Show("2");

            result.ExecuteResult(_mockControllerContext.Object);

            var document = TestHelper.LoadXml(_result.ToString());

            Assert.That(document.SelectSingleNode("Response/Gather/Say"), Is.Not.Null);
            Assert.That(document.SelectSingleNode("Response/Gather").Attributes["action"].Value, Is.Empty);
        }


        [Test]
        public void GivenAShowAction_WhenTheSelectedOptionIsDifferentThan1Or2_ThenTheResponseRedirectsToIVRWelcome()
        {
            var controller = new MenuController();
            var result = controller.Show("*");

            result.ExecuteResult(_mockControllerContext.Object);

            var document = TestHelper.LoadXml(_result.ToString());

            Assert.That(document.SelectSingleNode("Response/Redirect").InnerText, Is.EqualTo("/ivr/welcome"));
        }
    }
}
