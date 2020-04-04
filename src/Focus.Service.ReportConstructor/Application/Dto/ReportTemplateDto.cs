using System.Collections.Generic;
using System.Linq;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Focus.Service.ReportConstructor.Core.Entities.Table;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class ReportTemplateDto : ValueObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public QuestionnaireModuleTemplateDto[] Questionnaires { get; set; }
        public TableModuleTemplateDto[] Tables { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Title;

            foreach (var q in Questionnaires)
                yield return q;

            foreach (var t in Tables)
                yield return t;
        }
    }

    public static class ReportTemplateDtoExtensions
    {
        public static ReportTemplate AsEntity(this ReportTemplateDto dto)
        {
            return new ReportTemplate(
                id: dto.Id,
                title: dto.Title,
                modules: dto.Questionnaires
                                .Select(x => x.AsEntity() as IModuleTemplate)
                                .Concat(dto.Tables
                                    .Select(x => x.AsEntity() as IModuleTemplate))
                                .ToList());
        }

        public static ReportTemplateDto AsDto(this ReportTemplate entity)
        {
            var modules = entity.GetArray();

            return new ReportTemplateDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Questionnaires = modules
                    .OfType<QuestionnaireModuleTemplate>()
                    .Select(x => x.AsDto())
                    .ToArray(),
                Tables = modules
                    .OfType<TableModuleTemplate>()
                    .Select(x => x.AsDto())
                    .ToArray()
            };
        }
    }
}