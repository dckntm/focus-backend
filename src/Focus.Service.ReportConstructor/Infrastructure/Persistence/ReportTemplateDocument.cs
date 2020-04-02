using System.Collections.Generic;
using System.Linq;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Focus.Service.ReportConstructor.Core.Entities.Table;
using MongoDB.Bson;

namespace Focus.Service.ReportConstructor.Core.Persistence
{
    public class ReportTemplateDocument
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public ICollection<QuestionnaireModuleTemplate> Questionnaires { get; set; }
        public ICollection<TableModuleTemplate> Tables { get; set; }
    }

    public static class ReportTemplateDocumentExtensions
    {
        public static ReportTemplate AsEntity(this ReportTemplateDocument document)
            => new ReportTemplate(
                modules:
                    document.Questionnaires
                    .Select(x => x as ModuleTemplate)
                    .Concat(document.Tables)
                    .ToList(),
                id: document.Id.ToString(),
                title: document.Title
            );

        public static ReportTemplateDocument AsDocument(this ReportTemplate entity)
            => new ReportTemplateDocument()
            {
                Id = string.IsNullOrEmpty(entity.Id) ? ObjectId.GenerateNewId() : new ObjectId(entity.Id),
                Title = entity.Title,
                Questionnaires = entity.Modules is null ?
                new List<QuestionnaireModuleTemplate>() :
                entity
                    .Modules
                    .Where(x => x is QuestionnaireModuleTemplate)
                    .Select(x => x as QuestionnaireModuleTemplate)
                    .ToList(),
                Tables = entity.Modules is null ?
                new List<TableModuleTemplate>() :
                entity
                    .Modules
                    .Where(x => x is TableModuleTemplate)
                    .Select(x => x as TableModuleTemplate)
                    .ToList()
            };
    }
}