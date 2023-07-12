using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string DeviceName { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
        public DateTime? UpdatedDate { get; set; } = null;
    }
}
