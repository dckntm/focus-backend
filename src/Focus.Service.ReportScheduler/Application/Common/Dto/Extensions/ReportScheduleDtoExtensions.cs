using System;
using System.Globalization;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Application.Common.Dto.Extensions
{
    public static class ReportScheduleDtoExtensions
    {
        public static ReportSchedule AsEntity(this ReportScheduleDto dto)
        {
            var emissionStart = new DateTime();
            var emissionEnd = new DateTime();

            if (!DateTime.TryParseExact(dto.EmissionStart, "dd.MM.yyyy", null, DateTimeStyles.None, out emissionStart))
                throw new ArgumentException($"Can't parse {dto.EmissionStart}");

            if (!DateTime.TryParseExact(dto.EmissionEnd, "dd.MM.yyyy", null, DateTimeStyles.None, out emissionEnd))
                throw new ArgumentException($"Can't parse {dto.EmissionEnd}");

            return new ReportSchedule
            {
                Id = dto.Id,
                ReportTemplate = dto.ReportTemplate,
                Organizations = dto.Organizations,
                DeadlinePeriod = dto.DeadlinePeriod.AsPeriod(),
                EmissionPeriod = dto.EmissionPeriod.AsPeriod(),
                EmissionStart = emissionStart.ToUniversalTime(),
                EmissionEnd = emissionEnd.ToUniversalTime(),
            };
        }

        public static ReportScheduleDto AsDto(this ReportSchedule entity)
            => new ReportScheduleDto
            {
                Id = entity.Id,
                ReportTemplate = entity.ReportTemplate,
                Organizations = entity.Organizations,
                DeadlinePeriod = entity.DeadlinePeriod.ToString(),
                EmissionPeriod = entity.EmissionPeriod.ToString(),
                EmissionStart = entity.EmissionStart.ToLocalTime().ToString("dd.MM.yyyy"),
                EmissionEnd = entity.EmissionEnd.ToLocalTime().ToString("dd.MM.yyyy")
            };

        public static Period AsPeriod(this string str)
        {
            var subStrings = str.Split('.');

            if (subStrings.Length != 3)
                throw new InvalidOperationException($"Can't cast string {str} to Period object");

            var result = new Period();

            result.Days = Int32.Parse(subStrings[0]);
            result.Months = Int32.Parse(subStrings[1]);
            result.Years = Int32.Parse(subStrings[2]);

            return result;
        }
    }
}