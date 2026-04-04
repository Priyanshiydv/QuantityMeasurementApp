using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurement.QMAService.Models
{
    [Table("QuantityMeasurementEntity")]
    public class MeasurementEntity
    {
        [Key]
        public string Id { get; set; } =
            Guid.NewGuid().ToString();

        [MaxLength(500)]
        public string? FirstOperand { get; set; }

        [MaxLength(500)]
        public string? SecondOperand { get; set; }

        [Required]
        [MaxLength(50)]
        public string OperationType { get; set; }
            = string.Empty;

        [MaxLength(500)]
        public string? Result { get; set; }

        public bool HasError { get; set; } = false;

        [MaxLength(500)]
        public string? ErrorMessage { get; set; }

        [MaxLength(100)]
        public string? MeasurementType { get; set; }

        public DateTime Timestamp { get; set; }
            = DateTime.UtcNow;

        public int? UserId { get; set; }
    }
}