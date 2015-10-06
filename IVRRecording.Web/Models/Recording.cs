namespace IVRRecording.Web.Models
{
    public class Recording
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Transcription { get; set; }
        public string PhoneNumber { get; set; }

        public int AgentId { get; set; }
        public virtual Agent Agent { get; set; }
    }
}