using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportProcessor.Entities.Questionnaire
{
    public class SectionAnswer : ValueObject
    {
        public int Order { get; set; }
        public bool Repeatable { get; set; }
        public IList<QuestionAnswer> QuestionAnswers { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Order;
            yield return Repeatable;
            
            foreach (var q in QuestionAnswers)
                yield return q;
        }
    }
}