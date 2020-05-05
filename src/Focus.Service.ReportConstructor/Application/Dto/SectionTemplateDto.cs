using System.Collections.Generic;
using System.Linq;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class SectionTemplateDto : ValueObject
    {
        public string Title { get; set; }
        public int Order { get; set; }
        // public bool Repeatable { get; set; }
        public QuestionTemplateDto[] Questions { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Order;
            // yield return Repeatable;

            foreach (var q in Questions)
                yield return q;
        }
    }

    public static class SectionTemplateDtoExtensions
    {
        public static SectionTemplate AsEntity(this SectionTemplateDto dto)
        {
            return new SectionTemplate(
                title: dto.Title,
                // repeatable: dto.Repeatable,
                questions: dto.Questions
                    .Select(x => x.AsEntity())
                    .ToList(),
                order: dto.Order);
        }

        public static SectionTemplateDto AsDto(this SectionTemplate entity)
        {
            return new SectionTemplateDto()
            {
                Title = entity.Title,
                Order = entity.Order,
                // Repeatable = entity.Repeatable,
                Questions = entity
                    .GetArray()
                    .Select(x => x.AsDto())
                    .ToArray()
            };
        }
    }
}