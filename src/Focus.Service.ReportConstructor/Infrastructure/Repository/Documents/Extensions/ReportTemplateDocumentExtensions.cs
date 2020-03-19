using System;
using System.Collections.Generic;
using System.Linq;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Entities.Table;

namespace Focus.Service.ReportConstructor.Infrastructure.Repository.Documents.Extensions
{
    public static class ReportTemplateDocumentExtensions
    {
        public static ReportTemplate AsEntity(this ReportTemplateDocument document)
            => new ReportTemplate()
            {
                Id = document.Id.ToString(),
                Title = document.Title,
                Modules = new List<ModuleTemplate>()
                    .Concat(document.Questionnaires
                        .Select(x => x as QuestionnaireModuleTemplate))
                    .Concat(document.Tables
                        .Select(x => x as TableModuleTemplate))
                    .ToList()
            };

        public static ReportTemplateDocument AsDocument(this ReportTemplate entity)
            => new ReportTemplateDocument()
            {
                Id = entity.Id == "" ? Guid.NewGuid() : new Guid(entity.Id),
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