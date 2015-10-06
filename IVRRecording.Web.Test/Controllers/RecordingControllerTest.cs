using IVRRecording.Web.Controllers;
using IVRRecording.Web.Models;
using IVRRecording.Web.Models.Repository;
using Moq;
using NUnit.Framework;

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
            var result = controller.Create("1", "caller", "transcription", "url");

            result.ExecuteResult(MockControllerContext.Object);

            mockRepository.Verify(x => x.Create(It.IsAny<Recording>()), Times.Once);
            Assert.That(Result.ToString(), Is.EqualTo("Recording saved"));
        }
    }
}
