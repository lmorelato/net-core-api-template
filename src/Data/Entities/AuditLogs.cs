using System;

using Microsoft.EntityFrameworkCore;

namespace Template.Data.Entities
{
    public class AuditLogs : AutoHistory
    {
        public DateTime ModifiedOn { get; set; }

        public int ModifiedById { get; set; }
    }
}
