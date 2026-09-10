namespace backend.Domain.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string entityName, object id) : 
            base($"{entityName} с ID: {id} не найден.", 404, "NOT_FOUND")
        {

        }
    }
}
