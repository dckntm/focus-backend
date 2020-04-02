using System.Collections.Generic;
using Focus.Service.ReportConstructor.Core.Abstract;

namespace Focus.Service.ReportConstructor.Core.Entities.Questionnaire
{
    public class QuestionnaireModuleTemplate : ModuleTemplate
    {
        private ICollection<SectionTemplate> _sections;

        public QuestionnaireModuleTemplate() { }

        public QuestionnaireModuleTemplate(
            string title,
            int order,
            ICollection<SectionTemplate> sections)
        {
            Title = title;
            Order = order;

            // TODO: add validation for sections

            _sections = sections;
        }

        public ICollection<SectionTemplate> Sections
        {
            get => _sections;
            set => _sections = value;
            // set
            // {
            //     if (value is null || value.Count < 1)
            //         throw new InvalidStructureException("Questionnaire can't have no sections");

            //     _sections = value;
            // }
        }
    }
}