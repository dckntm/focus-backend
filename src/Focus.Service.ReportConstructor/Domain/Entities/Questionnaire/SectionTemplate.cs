using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Exceptions;

namespace Focus.Service.ReportConstructor.Domain.Entities.Questionnaires
{
    public class SectionTemplate
    {
        // fields
        private ICollection<QuestionTemplate> _questions;

        // properties
        public string Title { get; set; }
        public int Order { get; set; }
        public bool Repeatable { get; set; }
        public ICollection<QuestionTemplate> Questions
        {
            get => _questions;
            set
            {
                if (value is null || value.Count < 1)
                    throw new InvalidStructureException("Section can't have no questions");

                _questions = value;
            }
        }
    }
}