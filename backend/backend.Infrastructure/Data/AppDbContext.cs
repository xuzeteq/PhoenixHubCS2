using backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
        public DbSet<PromocodeUsage> PromocodeUsages { get; set; }
        public DbSet<Subscribtion> Subscribtions { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<PrivilegeFeature> PrivilegeFeatures { get; set; }
        public DbSet<AppLog> Logs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BalanceTransaction> BalanceTransactions { get; set; }
        public DbSet<UserPrivilege> UserPrivileges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Promocode>(entity =>
            {
                entity.ToTable("promocodes");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(24).IsRequired();
                entity.Property(e => e.GiveBalance).HasColumnName("give_balance").IsRequired();
                entity.Property(e => e.UsedCount).HasColumnName("used_count");
                entity.Property(e => e.MaxUses).HasColumnName("max_uses").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.ExpireAt).HasColumnName("expire_at");
            });

            modelBuilder.Entity<PromocodeUsage>(entity =>
            {
                entity.ToTable("promocode_usages");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PromocodeId).HasColumnName("promocode_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<Subscribtion>(entity =>
            {
                entity.ToTable("subscribtions");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.ExpireAt).HasColumnName("expire_at");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.ToTable("features");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Icon).HasColumnName("icon");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.ToTable("privileges");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.ImageUrl).HasColumnName("image_url");
                entity.Property(e => e.OldPrice).HasColumnName("old_price");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<PrivilegeFeature>(entity =>
            {
                entity.ToTable("privilege_features");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FeatureId).HasColumnName("feature_id");
                entity.Property(e => e.PrivilegeId).HasColumnName("privilege_id");
                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<AppLog>(entity =>
            {
                entity.ToTable("logs");
                entity.HasNoKey();

                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.MessageTemplate).HasColumnName("message_template");
                entity.Property(e => e.Level).HasColumnName("level");
                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
                entity.Property(e => e.Exception).HasColumnName("exception");
                entity.Property(e => e.Properties).HasColumnName("properties");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("audit_logs");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.Action);
                entity.HasIndex(e => e.EntityType);
                entity.HasIndex(e => new { e.UserId, e.Timestamp });

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Username).HasMaxLength(100).HasColumnName("username");
                entity.Property(e => e.Action).HasMaxLength(100).IsRequired().HasColumnName("action");
                entity.Property(e => e.EntityType).HasMaxLength(100).IsRequired().HasColumnName("entity_type");
                entity.Property(e => e.EntityId).HasMaxLength(100).HasColumnName("entity_id");
                entity.Property(e => e.EntityName).HasMaxLength(200).HasColumnName("entity_name");
                entity.Property(e => e.OldValue).HasColumnName("old_value");
                entity.Property(e => e.NewValue).HasColumnName("new_value");
                entity.Property(e => e.StatusCode).HasColumnName("status_code");
                entity.Property(e => e.IsSuccess).HasColumnName("is_success");
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000).HasColumnName("error_message");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Currency).HasMaxLength(10).HasColumnName("currency");
                entity.Property(e => e.DurationDays).HasMaxLength(10).HasColumnName("duration_days");
                entity.Property(e => e.ValidUntil).HasMaxLength(10).HasColumnName("valid_until");
                entity.Property(e => e.Metadata).HasMaxLength(10).HasColumnName("metadata");
                entity.Property(e => e.Timestamp).HasMaxLength(10).HasColumnName("timestamp");
            });

            modelBuilder.Entity<BalanceTransaction>(entity =>
            {
                entity.ToTable("balance_transactions");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.BalanceBefore).HasColumnName("balance_before");
                entity.Property(e => e.BalanceAfter).HasColumnName("balance_after");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
                entity.Property(e => e.Metadata).HasColumnName("metadata");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<UserPrivilege>(entity =>
            {
                entity.ToTable("user_privileges");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.PrivilegeId).HasColumnName("privilege_id");
                entity.Property(e => e.PurchasedAt).HasColumnName("purchased_at");
            });
        }
    }

        
}
