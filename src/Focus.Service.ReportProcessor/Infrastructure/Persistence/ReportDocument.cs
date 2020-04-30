using Focus.Service.ReportProcessor.Entities.Questionnaire;
using Focus.Service.ReportProcessor.Entities.Table;
using MongoDB.Bson.Serialization.Attributes;
using Focus.Service.ReportProcessor.Enums;
using System.Collections.Generic;
using MongoDB.Bson;
using System;
using Focus.Service.ReportProcessor.Entities;

namespace Focus.Service.ReportProcessor.Infrastructure.Persistence
{
    public class ReportDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ReportTemplateId { get; set; }
        public string AssignedOrganizationId { get; set; }
        public ReportStatus Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Deadline { get; set; }
        public IList<QuestionnaireModuleAnswer> QuestionnaireAnswers { get; set; }
        public IList<TableModuleAnswer> TableAnswers { get; set; }
    }

    public static class ReportDocumentExtensions
    {
        public static Report AsEntity(this ReportDocument doc)
        {
            return new Report()
            {
                Id = doc.Id.ToString(),
                ReportTemplateId = doc.ReportTemplateId,
                AssignedOrganizationId = doc.AssignedOrganizationId,
                Status = doc.Status,
                Deadline = doc.Deadline.ToLocalTime(),
                QuestionnaireAnswers = doc.QuestionnaireAnswers,
                TableAnswers = doc.TableAnswers
            };
        }

        public static ReportDocument AsDocument(this Report ent)
        {
            return new ReportDocument()
            {
                Id = string.IsNullOrEmpty(ent.Id) ? ObjectId.GenerateNewId() : new ObjectId(ent.Id),
                ReportTemplateId = ent.ReportTemplateId,
                AssignedOrganizationId = ent.AssignedOrganizationId,
                Status = ent.Status,
                Deadline = ent.Deadline.ToUniversalTime(),
                QuestionnaireAnswers = ent.QuestionnaireAnswers,
                TableAnswers = ent.TableAnswers
            };
        }
    }
}