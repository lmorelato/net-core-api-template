using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    public class EmailNotConfirmedException : BaseException, IKnownException
    {
        public EmailNotConfirmedException(string message)
            : base(message)
        {
        }
    }
}
