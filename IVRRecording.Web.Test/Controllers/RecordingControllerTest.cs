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
    public class RecordingControllerTest : ControllerTest
    {
        [TestCase]
        public void GivenACreateAction_ThenCreateIsCalledOnce()
        {
            var mockRepository = new Mock<IRecordingRepository>();
            mockRepository.Setup(r => r.Create(It.IsAny<Recording>()));

            var controller = new RecordingController(mockRepository.Object);
            controller.WithCallTo(c => c.Create("1", "caller", "transcription", "url"))
                .ShouldReturnTwiMLResult(r =>
                {
                    Assert.That(r.ToString(), Contains.Substring("Recording saved"));
                });

            mockRepository.Verify(x => x.Create(It.IsAny<Recording>()), Times.Once);
        }
    }
}
