using IVRRecording.Web.Controllers;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace IVRRecording.Web.Test.Controllers
{
    class IVRControllerTest : ControllerTest
    {
        [Test]
        public void GivenAWelcomeAction_ThenTheResponseContainsGatherPlay()
        {
            var controller = new IVRController();
            var result = controller.Welcome();

            result.ExecuteResult(MockControllerContext.Object);

            var document = LoadXml(Result.ToString());

            Assert.That(document.SelectSingleNode("Response/Gather/Play"), Is.Not.Null);
            Assert.That(document.SelectSingleNode("Response/Gather").Attributes["action"].Value, Is.Empty);
        }
    }
}
