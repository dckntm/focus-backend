using System;
using Focus.Service.ReportConstructor.Core.Abstract;
using Focus.Service.ReportConstructor.Core.Enums;

namespace Focus.Service.ReportConstructor.Core.Entities.Questionnaire
{
    public class QuestionTemplate : IOrderable
    {
        private string _questionText;
        private int _order;
        public InputType InputType { get; set; }
        public int Order
        {
            get => _order;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"DOMAIN EXCEPTION: Can't assign {value} to Question Template Order");

                _order = value;
            }
        }

        public string QuestionText
        {
            get => _questionText;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "DOMAIN EXCEPTION: Can't assign null, empty or whitespace Question Template Text");

                _questionText = value;
            }
        }

        public QuestionTemplate(
            string questionText,
            InputType inputType = InputType.ShortText,
            int order = 0)
        {
            if (string.IsNullOrEmpty(questionText) || string.IsNullOrWhiteSpace(questionText))
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Question Template with null, empty or whitespace Question Text");

            if (order < 0)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Question Template with {order} Order");

            _questionText = questionText;
            InputType = inputType;
            _order = order;
        }
    }
}