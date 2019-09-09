using System.Collections.Generic;

namespace Template.Core.Models
{
    public class EmailSettings
    {
        public EmailSettings()
        {
            this.Variables = new Dictionary<string, string>();
        }

        public string ToEmail { get; set; }

        public string ToName { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string TemplateId { get; set; }

        public Dictionary<string, string> Variables { get; set; }

        public bool HasTemplate => !string.IsNullOrWhiteSpace(this.TemplateId);
    }
}
