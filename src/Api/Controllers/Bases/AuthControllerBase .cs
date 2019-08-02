using Microsoft.AspNetCore.Authorization;

using Template.Shared.Session;

namespace Template.Api.Controllers.Bases
{
    [Authorize]
    public abstract class AuthControllerBase : AppControllerBase
    {
        protected AuthControllerBase(ICurrentSession currentSession)
        {
            this.CurrentSession = currentSession;
        }

        protected ICurrentSession CurrentSession { get; }
    }
}