using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportProcessor.Enums;

namespace Focus.Service.ReportProcessor.Entities.Questionnaire
{
    public class QuestionAnswer : ValueObject
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string Answer { get; set; }
        public InputType AnswerType { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Order;
            yield return Answer;
            yield return AnswerType;
        }
    }
}