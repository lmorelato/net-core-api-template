using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    public class NotFoundException : BaseException, IKnownException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
