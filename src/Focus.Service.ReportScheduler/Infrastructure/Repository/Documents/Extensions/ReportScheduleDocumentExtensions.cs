using System;
using Focus.Service.ReportScheduler.Domain.Entities;

namespace Focus.Service.ReportScheduler.Infrastructure.Repository.Documents.Extensions
{
    public static class ReportScheduleDocumentExtensions
    {
        public static ReportSchedule AsEntity(this ReportScheduleDocument document)
            => new ReportSchedule
            {
                Id = document.Id.ToString(),
                ReportTemplate = document.ReportTemplate,
                Organizations = document.Organizations,
                DeadlinePeriod = document.DeadlinePeriod,
                EmissionPeriod = document.EmissionPeriod,
                EmissionStart = document.EmissionStart,
                EmissionEnd = document.EmissionEnd
            };

        public static ReportScheduleDocument AsDocument(this ReportSchedule entity)
            => new ReportScheduleDocument
            {
                Id = new Guid(entity.Id),
                ReportTemplate = entity.ReportTemplate,
                Organizations = entity.Organizations,
                DeadlinePeriod = entity.DeadlinePeriod,
                EmissionPeriod = entity.EmissionPeriod,
                EmissionStart = entity.EmissionStart,
                EmissionEnd = entity.EmissionEnd
            };
    }
}