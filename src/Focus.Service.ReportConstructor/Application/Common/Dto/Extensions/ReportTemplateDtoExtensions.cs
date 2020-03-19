using System.Collections.Generic;
using System.Linq;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Entities.Table;

namespace Focus.Service.ReportConstructor.Application.Common.Dto.Extensions
{
    public static class ReportTemplateDtoExtensions
    {
        public static ReportTemplate AsEntity(this ReportTemplateDto dto)
            => new ReportTemplate()
            {
                Id = dto.Id,
                Title = dto.Title,
                Modules = new List<ModuleTemplate>()
                    .Concat(dto.Questionnaires.Select(x => x as ModuleTemplate))
                    .Concat(dto.Tables.Select(x => x as ModuleTemplate))
                    .ToList()
            };

        public static ReportTemplateDto AsDto(this ReportTemplate entity)
        {
            if (entity.Modules is null || entity.Modules.Count < 1)
            {
                return new ReportTemplateDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Questionnaires = new List<QuestionnaireModuleTemplate>(),
                    Tables = new List<TableModuleTemplate>()
                };
            }
            else
            {
                var questionnaires = entity.Modules
                    .Where(x => x is QuestionnaireModuleTemplate)
                    .Select(x => x as QuestionnaireModuleTemplate) ?? new List<QuestionnaireModuleTemplate>();

                var tables = entity.Modules
                    .Where(x => x is TableModuleTemplate)
                    .Select(x => x as TableModuleTemplate) ?? new List<TableModuleTemplate>();

                return new ReportTemplateDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Questionnaires = questionnaires.ToList(),
                    Tables = tables.ToList()
                };
            }
        }
    }
}