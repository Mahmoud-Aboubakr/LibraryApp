

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string DeviceName { get; set; } = Environment.MachineName.ToString();
        [Column(TypeName ="datetime")]
        public DateTime? CreatedDate { get; set; } = null;
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; } = null;
    }
}
