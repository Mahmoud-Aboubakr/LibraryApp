using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Vacation
{
    public class CreateVacationDto
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DayDate { get; set; }
        public bool? NormalVacation { get; set; }
        public bool? UrgentVacation { get; set; }
        public bool? Absence { get; set; }

        public int EmpId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
