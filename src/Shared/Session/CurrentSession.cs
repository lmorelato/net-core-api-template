﻿using System.Collections.Generic;

namespace Template.Shared.Session
{
    public class CurrentSession : ICurrentSession
    {
        public int UserId { get; set; }

        public int? TenantId { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public string UserName { get; set; }

        public bool DisableTenantFilter { get; set; }

        public bool DisableSoftDeleteFilter { get; set; }
    }
}
