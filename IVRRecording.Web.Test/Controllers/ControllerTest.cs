using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Moq;
using NUnit.Framework;

namespace IVRRecording.Web.Test.Controllers
{
    public class ControllerTest
    {
        protected StringBuilder Result;
        protected Mock<ControllerContext> MockControllerContext;

        [SetUp]
        public void Setup()
        {
            Result = new StringBuilder();
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(s => s.Write(It.IsAny<string>())).Callback<string>(c => Result.Append(c));
            mockResponse.Setup(s => s.Output).Returns(new StringWriter(Result));

            MockControllerContext = new Mock<ControllerContext>();
            MockControllerContext.Setup(x => x.HttpContext.Response).Returns(mockResponse.Object);
        }

        public XmlDocument LoadXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);

            return document;
        }
    }
}