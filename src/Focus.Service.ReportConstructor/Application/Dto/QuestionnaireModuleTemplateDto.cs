using System.Collections.Generic;
using System.Linq;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class QuestionnaireModuleTemplateDto : ValueObject
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public SectionTemplateDto[] Sections { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Order;

            foreach (var s in Sections)
                yield return s;
        }
    }

    public static class QuestionnaireModuleTemplateDtoExtensions
    {
        public static QuestionnaireModuleTemplate AsEntity(this QuestionnaireModuleTemplateDto dto)
        {
            return new QuestionnaireModuleTemplate(
                title: dto.Title,
                sections: dto.Sections
                            .Select(x => x.AsEntity())
                            .ToList(),
                order: dto.Order);
        }

        public static QuestionnaireModuleTemplateDto AsDto(this QuestionnaireModuleTemplate entity)
        {
            return new QuestionnaireModuleTemplateDto
            {
                Title = entity.Title,
                Order = entity.Order,
                Sections = entity
                    .GetArray()
                    .Select(x => x.AsDto())
                    .ToArray(),
            };
        }
    }
}