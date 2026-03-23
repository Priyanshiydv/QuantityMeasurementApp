using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.Models.Entities;

namespace QuantityMeasurement.Repository.Context
{
    /// <summary>
    /// Entity Framework Core DbContext for
    /// Quantity Measurement Application.
    /// Manages database connection and entity mappings.
    /// Replaces manual ADO.NET from UC16.
    /// UC17
    /// </summary>
    public class QuantityMeasurementDbContext : DbContext
    {
        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Constructor with DbContextOptions.
        /// Options injected by ASP.NET Core DI Container.
        /// Supports both InMemory and SQL Server databases.
        /// UC17
        /// </summary>
        public QuantityMeasurementDbContext(
            DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options)
        {
            Console.WriteLine(
                "[DbContext] Initialized ✓");
        }

        // ─── DbSets ───────────────────────────────────────────

        /// <summary>
        /// DbSet for QuantityMeasurementEntity.
        /// Represents QuantityMeasurements table in database.
        /// Used for all CRUD operations via EF Core.
        /// UC17
        /// </summary>
        public DbSet<QuantityMeasurementEntity>
            QuantityMeasurements { get; set; }

        // ─── Model Configuration ──────────────────────────────

        /// <summary>
        /// Configures entity mappings and table structure.
        /// Called by EF Core when building the model.
        /// UC17
        /// </summary>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure QuantityMeasurementEntity table
            modelBuilder.Entity<QuantityMeasurementEntity>(
                entity =>
                {
                    // Table name in database
                    entity.ToTable("QuantityMeasurementEntity");

                    // Primary key
                    entity.HasKey(e => e.Id);

                    // Id column
                    entity.Property(e => e.Id)
                        .HasMaxLength(50)
                        .IsRequired();

                    // FirstOperand column
                    entity.Property(e => e.FirstOperand)
                        .HasMaxLength(200)
                        .IsRequired(false);

                    // SecondOperand column
                    entity.Property(e => e.SecondOperand)
                        .HasMaxLength(200)
                        .IsRequired(false);

                    // OperationType column
                    entity.Property(e => e.OperationType)
                        .HasMaxLength(50)
                        .IsRequired();

                    // Result column
                    entity.Property(e => e.Result)
                        .HasMaxLength(200)
                        .IsRequired(false);

                    // HasError column
                    entity.Property(e => e.HasError)
                        .IsRequired()
                        .HasDefaultValue(false);

                    // ErrorMessage column
                    entity.Property(e => e.ErrorMessage)
                        .HasMaxLength(500)
                        .IsRequired(false);

                    // MeasurementType column
                    entity.Property(e => e.MeasurementType)
                        .HasMaxLength(50)
                        .IsRequired(false);

                    // Timestamp column
                    entity.Property(e => e.Timestamp)
                        .IsRequired();

                    // Index on OperationType
                    entity.HasIndex(e => e.OperationType)
                        .HasDatabaseName("IX_OperationType");

                    // Index on MeasurementType
                    entity.HasIndex(e => e.MeasurementType)
                        .HasDatabaseName("IX_MeasurementType");

                    // Index on Timestamp
                    entity.HasIndex(e => e.Timestamp)
                        .HasDatabaseName("IX_Timestamp");

                    // Index on HasError
                    entity.HasIndex(e => e.HasError)
                        .HasDatabaseName("IX_HasError");
                });

            Console.WriteLine(
                "[DbContext] Entity mappings configured ✓");
        }

        // ─── Override SaveChanges ─────────────────────────────

        /// <summary>
        /// Override SaveChanges to auto set Timestamp.
        /// Called before every save operation.
        /// UC17
        /// </summary>
        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync for async operations.
        /// UC17
        /// </summary>
        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        // ─── Private Helper ───────────────────────────────────

        /// <summary>
        /// Sets Timestamp for new entities automatically.
        /// Called before SaveChanges.
        /// UC17
        /// </summary>
        private void SetTimestamps()
        {
            var newEntities = ChangeTracker
                .Entries<QuantityMeasurementEntity>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entity in newEntities)
            {
                if (entity.Entity.Timestamp == default)
                {
                    entity.Entity.Timestamp = DateTime.Now;
                }
            }
        }
    }
}