using System;

using Microsoft.AspNetCore.Identity;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Entities.Identity
{
    public sealed class User : IdentityUser<int>, ISoftDelete
    {
        public string FullName { get; set; }

        public string Culture { get; set; } = "en-US";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
    }
}