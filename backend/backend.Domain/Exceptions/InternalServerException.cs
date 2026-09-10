namespace backend.Domain.Exceptions
{
    public class InternalServerException : BaseException
    {
        public InternalServerException(string message, string errorCode) : 
            base(message, 500, errorCode) { }
    }
}
