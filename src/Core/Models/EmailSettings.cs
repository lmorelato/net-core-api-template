using System.Collections.Generic;

namespace Template.Core.Models
{
    public class EmailSettings
    {
        public EmailSettings()
        {
            this.Variables = new Dictionary<string, string>();
        }

        public string Subject { get; set; }

        public string ToEmail { get; set; }

        public string ToName { get; set; }

        public int? TemplateId { get; set; }

        public Dictionary<string, string> Variables { get; set; }
    }
}
