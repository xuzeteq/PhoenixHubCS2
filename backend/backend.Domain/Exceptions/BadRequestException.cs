namespace backend.Domain.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message, string errorCode) : 
            base(message, 400, errorCode) { }
    }
}
