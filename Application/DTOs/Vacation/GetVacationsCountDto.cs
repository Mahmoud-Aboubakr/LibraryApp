using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Vacation
{
    public class GetVacationsCountDto
    {
        public int EmpId { get; set; }
        public int NormalVacationCount { get; set; }
        public int UrgentVacationCount { get; set; }
        public int AbsenceCount { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } 
    }
}
