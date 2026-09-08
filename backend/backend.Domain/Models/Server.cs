using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain.Models
{
    [Table("servers")]
    public class Server
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("title")]
        public string? Title { get; set; } = string.Empty;

        [Column("map")]
        public string? Map { get; set; } = string.Empty;
        
        [Column("ip_address")]
        public string IpAddress { get; set; } = string.Empty;
        
        [Column("port")]
        public ushort Port { get; set; }
        
        [Column("online")]
        public int? Online { get; set; }
        
        [Column("max_online")]
        public int? MaxOnline { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
