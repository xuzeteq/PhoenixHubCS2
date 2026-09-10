namespace backend.Domain.Exceptions
{
    public class BaseException : System.Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        public BaseException(string message, int statusCode, string errorCode) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
