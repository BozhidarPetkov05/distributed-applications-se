using System.ComponentModel.DataAnnotations;

namespace CarSales.Data.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime LastChange { get; set; }
    }
}
