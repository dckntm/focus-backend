using System;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;

namespace Focus.Service.ReportConstructor.Core.Entities.Questionnaire
{
    public class QuestionnaireModuleTemplate : ListContainer<SectionTemplate>, ITitled, IOrderable
    {
        private string _title;
        private int _order;

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "DOMAIN EXCEPTION: Can't assign null, empty or whitespace Questionnaire Module Template Title");

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
                        $"DOMAIN EXCEPTION: Can't assign {value} to Questionnaire Module Template");

                _order = value;
            }
        }

        public QuestionnaireModuleTemplate(
            string title,
            IList<SectionTemplate> sections,
            int order = 0)
        {
            if (sections is null || sections.Count < 1)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Questionnaire Module Template with null or empty Section Template collection");

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Questionnaire Module Template with null, empty or whitespace Title");

            if (order < 0)
                throw new ArgumentException(
                    "DOMAIN EXCEPTION: Can't initialize Questionnaire Module Template with {order} Order");

            Title = title;
            Order = order;
            _collection = sections;

            UpdateOrder();
        }
    }
}