using System;
using Focus.Service.ReportConstructor.Domain.Enums;

namespace Focus.Service.ReportConstructor.Domain.Entities.Questionnaires
{
    public class QuestionTemplate
    {
        // fields
        private string _questionText;

        // properties
        public string QuestionText
        {
            get => _questionText;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Question text can't be null, empty or whitespace");

                _questionText = value;
            }
        }
        public InputType InputType { get; set; }
    }
}