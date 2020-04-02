using Focus.Service.ReportConstructor.Core.Enums;

namespace Focus.Service.ReportConstructor.Core.Entities.Questionnaire
{
    public class QuestionTemplate
    {
        public QuestionTemplate() { }

        public QuestionTemplate(string questionText, InputType inputType)
        {
            // TODO: validation for 

            QuestionText = questionText;
            InputType = inputType;
        }

        public string QuestionText { get; set; }
        public InputType InputType { get; set; }
    }
}