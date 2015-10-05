using System.Xml;

namespace IVRRecording.Web.Test
{
    public class TestHelper
    {
        public static XmlDocument LoadXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);

            return document;
        }
    }
}
