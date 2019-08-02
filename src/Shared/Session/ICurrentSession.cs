using System.Collections.Generic;

namespace Template.Shared.Session
{
    public interface ICurrentSession
    {
        int UserId { get; set; }

        int TenantId { get; set; }

        List<string> Roles { get; set; }

        string UserName { get; set; }

        bool DisableTenantFilter { get; set; }

        bool DisableSoftDeleteFilter { get; set; }
    }
}
