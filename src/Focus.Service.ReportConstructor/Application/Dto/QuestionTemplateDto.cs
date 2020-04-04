using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportConstructor.Core.Entities.Questionnaire;
using Focus.Service.ReportConstructor.Core.Enums;

namespace Focus.Service.ReportConstructor.Application.Dto
{
    public class QuestionTemplateDto : ValueObject
    {
        public string QuestionText { get; set; }
        public InputType InputType { get; set; }
        public int Order { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return QuestionText;
            yield return InputType;
            yield return Order;
        }
    }

    public static class QuestionTemplateDtoExtensions
    {
        public static QuestionTemplate AsEntity(this QuestionTemplateDto dto)
        {
            return new QuestionTemplate(
                questionText: dto.QuestionText,
                inputType: dto.InputType,
                order: dto.Order);
        }

        public static QuestionTemplateDto AsDto(this QuestionTemplate entity)
        {
            return new QuestionTemplateDto()
            {
                QuestionText = entity.QuestionText,
                InputType = entity.InputType,
                Order = entity.Order
            };
        }
    }
}