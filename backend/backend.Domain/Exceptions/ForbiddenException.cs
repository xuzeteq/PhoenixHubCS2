namespace backend.Domain.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message, string errorCode) :
            base(message, 403, errorCode) { }
    }
}
