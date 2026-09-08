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
        }
    }

        
}
