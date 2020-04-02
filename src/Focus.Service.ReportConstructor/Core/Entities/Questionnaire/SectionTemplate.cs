using System.Collections.Generic;

namespace Focus.Service.ReportConstructor.Core.Entities.Questionnaire
{
    public class SectionTemplate
    {
        public SectionTemplate() { }

        public SectionTemplate(
            string title,
            string order,
            bool repeatable,
            ICollection<QuestionTemplate> questions)
        {
            // TODO: add validation for questions collection, order, etc.

            Title = title;
            Order = order;
            Repeatable = repeatable;
            Questions = questions;
        }

        public string Title { get; set; }
        public string Order { get; set; }
        public bool Repeatable { get; set; }
        public ICollection<QuestionTemplate> Questions { get; set; }
    }
}