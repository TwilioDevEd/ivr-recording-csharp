using System.Collections.Generic;

namespace IVRRecording.Web.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public string Extension { get; set; }
        public string PhoneNumber { get; set; }

        public virtual IList<Recording> Recordings { get; set; }
    }
}