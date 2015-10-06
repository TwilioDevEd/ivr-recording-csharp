using IVRRecording.Web.Controllers;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace IVRRecording.Web.Test.Controllers
{
    public class AgentControllerTest : ControllerTest
    {
        [Test]
        public void GivenACallAction_WhenStatusIsDifferentThanCompleted_ThenRecordTheCallAndHangup()
        {
            var controller = new AgentController();
            var result = controller.Call("busy");

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Record").Attributes["action"].Value,
                Is.EqualTo("/Agent/Hangup"));
            Assert.That(document.SelectSingleNode("Response/Hangup"), Is.Not.Null);
        }

        [Test]
        public void GivenACallAction_WhenStatusIsCompleted_ThenResponseWillBeEmpty()
        {
            var controller = new AgentController();
            var result = controller.Call("completed");

            result.ExecuteResult(MockControllerContext.Object);

            Assert.That(Result.ToString(), Is.Empty);
        }

        [Test]
        public void GivenAScreenCallAction_ThenResponseContainsGatherAndHangup()
        {
            var controller = new AgentController();
            var result = controller.ScreenCall("1234567890");

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Gather").Attributes["action"].Value,
                Is.EqualTo("/Agent/ConnectMessage"));
            Assert.That(document.SelectSingleNode("Response/Hangup"), Is.Not.Null);
        }

        [Test]
        public void GivenAScreenCallAction_ThenResponseContainsSpelledPhoneNumber()
        {
            var controller = new AgentController();
            var result = controller.ScreenCall("1234567890");

            result.ExecuteResult(MockControllerContext.Object);

            StringAssert.Contains("1, 2, 3, 4, 5, 6, 7, 8, 9, 0", Result.ToString());
        }

        [Test]
        public void GivenAConnectMessage_ThenRespondsWithInstructions()
        {
            var controller = new AgentController();
            var result = controller.ConnectMessage();

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Say"), Is.Not.Null);
        }

        [Test]
        public void GivenAHangupAction_ThenRespondsWithMessageAndHangup()
        {
            var controller = new AgentController();
            var result = controller.Hangup();

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Say"), Is.Not.Null);
            Assert.That(document.SelectSingleNode("Response/Hangup"), Is.Not.Null);
        }
    }
}
