

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string DeviceName { get; set; } = Environment.MachineName.ToString();
        public DateTime? CreatedDate { get; set; } = null;
        public DateTime? UpdatedDate { get; set; } = null;
    }
}
