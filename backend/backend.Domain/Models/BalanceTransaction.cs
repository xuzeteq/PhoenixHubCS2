using backend.Domain.Enums.Balance;

namespace backend.Domain.Models
{
    public class BalanceTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public BalanceTransactionTypeEnum Type { get; set; }

        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }

        public string? Description { get; set; }
        public int? ReferenceId { get; set; }
        public string? Metadata { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
