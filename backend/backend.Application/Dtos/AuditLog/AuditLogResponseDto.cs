namespace backend.Application.Dtos.AuditLog
{
    public class AuditLogResponseDto
    {
        public long Id { get; set; }

        public int? UserId { get; set; }
        public string? Username { get; set; }

        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public decimal? Amount { get; set; }
        public string? Currency { get; set; }

        public int? DurationDays { get; set; }
        public DateTime? ValidUntil { get; set; }

        public string? Metadata { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
