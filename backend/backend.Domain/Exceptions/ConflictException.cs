namespace backend.Domain.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string entityName, string title) : 
            base($"{entityName} с названием: {title} уже есть в базе данных.", 409, "CONFLICT")
        {

        }
    }
}
