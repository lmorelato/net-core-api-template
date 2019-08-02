using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    public class IdentityResultException : BaseException, IKnownException
    {
        public IdentityResultException(IdentityResult result)
        {
            this.Errors = result.Errors;
        }

        public IEnumerable<IdentityError> Errors { get; }
    }
}
