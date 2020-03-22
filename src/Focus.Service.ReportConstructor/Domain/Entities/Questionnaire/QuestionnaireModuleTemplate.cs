using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Exceptions;

namespace Focus.Service.ReportConstructor.Domain.Entities.Questionnaires
{
    public class QuestionnaireModuleTemplate : ModuleTemplate
    {
        // fields
        private ICollection<SectionTemplate> _sections;

        // properties
        public ICollection<SectionTemplate> Sections
        {
            get => _sections;
            set
            {
                if (value is null || value.Count < 1)
                    throw new InvalidStructureException("Questionnaire can't have no sections");

                _sections = value;
            }
        }
    }
}