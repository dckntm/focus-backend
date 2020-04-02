using System;
using System.Collections.Generic;
using System.Globalization;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportScheduler.Core.Entities;

namespace Focus.Service.ReportScheduler.Application.Dto
{
    public class ReportScheduleDto : ValueObject
    {
        public string Id { get; set; }
        public string ReportTemplate { get; set; }
        public ICollection<Assignment> Organizations { get; set; }
        // string in D.M.Y format
        public string DeadlinePeriod { get; set; }
        // string in D.M.Y format
        public string EmissionPeriod { get; set; }
        // string with DateTime format dd.MM.yyyy (considering time is local)
        public string EmissionStart { get; set; }
        // string with DateTime format dd.MM.yyyy (considering time is local)
        public string EmissionEnd { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return ReportTemplate;
            yield return DeadlinePeriod;
            yield return EmissionPeriod;
            yield return EmissionStart;
            yield return EmissionEnd;

            foreach (var org in Organizations)
                yield return org;
        }
    }

    public static class ReportScheduleDtoExtensions
    {
        public static ReportSchedule AsEntity(this ReportScheduleDto dto)
        {
            if (!DateTime.TryParseExact(dto.EmissionStart, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime emissionStart))
                throw new ArgumentException($"Can't parse {dto.EmissionStart}");

            if (!DateTime.TryParseExact(dto.EmissionEnd, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime emissionEnd))
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

            var result = new Period(
                days: Int32.Parse(subStrings[0]),
                months: Int32.Parse(subStrings[1]),
                years: Int32.Parse(subStrings[2]));

            return result;
        }
    }
}