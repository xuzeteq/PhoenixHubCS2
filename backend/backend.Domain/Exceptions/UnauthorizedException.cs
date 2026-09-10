namespace backend.Domain.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message, string errorCode) : 
            base(message, 401, errorCode) { }
    }
}
