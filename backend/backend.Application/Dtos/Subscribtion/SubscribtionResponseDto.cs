namespace backend.Application.Dtos.Subscribtion
{
    public class SubscribtionResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
