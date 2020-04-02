using System;
using System.Collections.Generic;
using Focus.Service.ReportScheduler.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Focus.Service.ReportScheduler.Infrastructure.Persistence
{
    public class ReportScheduleDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ReportTemplate { get; set; }
        public ICollection<Assignment> Organizations { get; set; }
        public Period DeadlinePeriod { get; set; }
        public Period EmissionPeriod { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EmissionStart { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EmissionEnd { get; set; }
    }

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
                Id = string.IsNullOrEmpty(entity.Id) ? ObjectId.GenerateNewId() : new ObjectId(entity.Id),
                ReportTemplate = entity.ReportTemplate,
                Organizations = entity.Organizations,
                DeadlinePeriod = entity.DeadlinePeriod,
                EmissionPeriod = entity.EmissionPeriod,
                EmissionStart = entity.EmissionStart,
                EmissionEnd = entity.EmissionEnd
            };
    }
}