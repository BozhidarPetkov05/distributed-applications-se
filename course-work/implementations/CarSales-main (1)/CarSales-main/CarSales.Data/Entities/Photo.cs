using System.ComponentModel.DataAnnotations.Schema;

namespace CarSales.Data.Entities
{
    public class Photo : BaseEntity
    {
        public required string ImagePath { get; set; }
        public bool IsMain { get; set; }
        public int ImageOrder {  get; set; }
        public virtual Car Car { get; set; }
        
        [ForeignKey(nameof(Car))]
        public Guid CarId { get; set; }
    }
}
