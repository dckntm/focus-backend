using System;
using System.Linq;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;

namespace Focus.Service.ReportConstructor.Core.Entities.Questionnaire
{
    public class SectionTemplate : ListContainer<QuestionTemplate>, IOrderable, ITitled
    {
        private int _order;
        private string _title;
        // public bool Repeatable { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "DOMAIN EXCEPTION: Can't assign null, empty or whitespace Section Template Title");

                _title = value;
            }
        }
        public int Order
        {
            get => _order;
            set
            {
                if (value < 0)
                    throw new ArgumentException(
                        $"DOMAIN EXCEPTION: Can't assign {value} to Section Template Order");

                _order = value;
            }
        }

        public SectionTemplate(
            string title,
            // bool repeatable,
            IList<QuestionTemplate> questions,
            int order = 0)
        {
            if (questions is null || questions.Count < 1)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Section Template with null or empty Question Template collection");

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Section Template with null, empty or whitespace Title");

            if (order < 0)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Section Template with {order} Order");

            Title = title;
            Order = order;
            // Repeatable = repeatable;
            _collection = questions;

            UpdateOrder();
        }
    }
}