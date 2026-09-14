namespace backend.Domain.Models
{
    public class UserPrivilege
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PrivilegeId { get; set; }
        public DateTime PurchasedAt { get; set; }

        public User User { get; set; } = null!;
        public Privilege Privilege { get; set; } = null!;
    }
}
