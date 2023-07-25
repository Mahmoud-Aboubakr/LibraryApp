using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Vacation
{
    public class ReadVacationDto
    {
        public int Id { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DayDate { get; set; }
        public bool? NormalVacation { get; set; }
        public bool? UrgentVacation { get; set; }
        public bool? Absence { get; set; }

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
