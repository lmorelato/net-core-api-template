using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    public class InvalidPasswordException : BaseException, IKnownException
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}
