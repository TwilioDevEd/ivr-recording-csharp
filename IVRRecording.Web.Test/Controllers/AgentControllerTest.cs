using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.XPath;
using IVRRecording.Web.Controllers;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;
using IVRRecording.Web.Test.Extensions;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

// ReSharper disable PossibleNullReferenceException

namespace IVRRecording.Web.Test.Controllers
{
    public class AgentControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_ThenReturnsAllAgents()
        {
            var brodo = new Agent {Extension = "Brodo"};
            var dagobah = new Agent {Extension = "Dagobah"};
            var agents = new List<Agent> {brodo, dagobah};

            var mockRepository = new Mock<IAgentRepository>();
            mockRepository.Setup(r => r.All()).Returns(agents);

            var controller = GetAgentController(mockRepository.Object);

            var result = controller.Index() as ViewResult;
            var model = (IList<Agent>) result.ViewData.Model;

            Assert.That(model.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(model, dagobah);
        }

        [Test]
        public void GivenACallAction_WhenStatusIsDifferentThanCompleted_ThenRecordTheCallAndHangup()
        {
            var controller = new AgentController {Url = Url};
            var result = controller.Call("1", "busy");

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Record").Attributes["action"].Value,
                Is.EqualTo("/Agent/Hangup"));
            Assert.That(document.SelectSingleNode("Response/Record").Attributes["transcribeCallback"].Value,
                Is.EqualTo("/Recording/Create?agentId=1"));
            Assert.That(document.SelectSingleNode("Response/Hangup"), Is.Not.Null);
        }

        [Test]
        public void GivenACallAction_WhenStatusIsCompleted_ThenResponseWillBeEmpty()
        {
            var controller = new AgentController();
            controller.WithCallTo(c => c.Call("1", "completed"))
                .ShouldReturnTwiMLResult(r =>
                {
                    Assert.That(r.XPathSelectElement("Root").Value.Trim(), Is.Empty);
                });
        }

        [Test]
        public void GivenAScreenCallAction_ThenResponseContainsGatherAndHangup()
        {
            var controller = new AgentController{Url = Url};
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
            var controller = new AgentController {Url = Url};
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

        private static AgentController GetAgentController(IAgentRepository repository)
        {
            var controller = new AgentController(repository);

            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return controller;
        }


        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(
                new GenericIdentity("someUser"), null /* roles */);

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }
    }
}
