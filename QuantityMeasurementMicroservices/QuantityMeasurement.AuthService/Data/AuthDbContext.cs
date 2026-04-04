using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.AuthService.Models;

namespace QuantityMeasurement.AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username)
                    .HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email)
                    .HasMaxLength(200).IsRequired();
                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(256).IsRequired();
                entity.Property(e => e.Role)
                    .HasMaxLength(50).HasDefaultValue("User");
                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(512);
                entity.Property(e => e.GoogleId)
                    .HasMaxLength(200);
                entity.Property(e => e.GoogleEmail)
                    .HasMaxLength(200);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}