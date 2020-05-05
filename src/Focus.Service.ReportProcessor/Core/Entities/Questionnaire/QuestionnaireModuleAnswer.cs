using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportProcessor.Entities.Questionnaire
{
    public class QuestionnaireModuleAnswer : ValueObject
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<SectionAnswer> SectionAnswers { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Order;

            foreach (var s in SectionAnswers)
                yield return s;
        }
    }
}