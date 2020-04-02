using System.Collections.Generic;
using System.Linq;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Entities;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Focus.Service.ReportConstructor.Core.Entities.Table;

namespace Focus.Service.ReportConstructor.Application.Common.Dto
{
    public class ReportTemplateDto : ValueObject
    {
        // properties
        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<QuestionnaireModuleTemplate> Questionnaires { get; set; }
        public ICollection<TableModuleTemplate> Tables { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Title;

            // TODO: implement ValueObject for domain entities of report constructor service
            //       to simplify this method with recursive value object equality comparison

            foreach (var Q in Questionnaires.OrderBy(x => x.Order))
            {
                yield return Q.Title;
                yield return Q.Order;

                foreach (var s in Q.Sections.OrderBy(x => x.Order))
                {
                    yield return s.Title;
                    yield return s.Repeatable;
                    yield return s.Order;

                    foreach (var q in s.Questions.OrderBy(x => x.QuestionText))
                    {
                        yield return q.QuestionText;
                        yield return q.InputType;
                    }
                }
            }

            foreach (var T in Tables.OrderBy(x => x.Order))
            {
                yield return T.Title;
                yield return T.Order;

                foreach (var c in T.Cells)
                {
                    yield return c.Column;
                    yield return c.ColumnSpan;
                    yield return c.Row;
                    yield return c.RowSpan;
                    yield return c.DefaultValue;
                    yield return c.InputType;
                }
            }
        }
    }

    public static class ReportTemplateDtoExtensions
    {
        public static ReportTemplate AsEntity(this ReportTemplateDto dto)
            => new ReportTemplate(
                modules: new List<ModuleTemplate>()
                                .Concat(dto.Questionnaires.Select(x => x as ModuleTemplate))
                                .Concat(dto.Tables.Select(x => x as ModuleTemplate))
                                .ToList(),
                id: dto.Id,
                title: dto.Title
            );

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