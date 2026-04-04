using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.QMAService.Models;

namespace QuantityMeasurement.QMAService.Data
{
    public class QMADbContext : DbContext
    {
        public QMADbContext(
            DbContextOptions<QMADbContext> options)
            : base(options) { }

        public DbSet<MeasurementEntity> Measurements
            { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasurementEntity>(entity =>
            {
                entity.ToTable("QuantityMeasurementEntity");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OperationType)
                    .HasMaxLength(50).IsRequired();
                entity.Property(e => e.FirstOperand)
                    .HasMaxLength(500);
                entity.Property(e => e.SecondOperand)
                    .HasMaxLength(500);
                entity.Property(e => e.Result)
                    .HasMaxLength(500);
                entity.Property(e => e.MeasurementType)
                    .HasMaxLength(100);
                entity.Property(e => e.UserId)
                    .IsRequired(false);
                entity.HasIndex(e => e.OperationType)
                    .HasDatabaseName("IX_OperationType");
                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_UserId");
            });
        }
    }
}