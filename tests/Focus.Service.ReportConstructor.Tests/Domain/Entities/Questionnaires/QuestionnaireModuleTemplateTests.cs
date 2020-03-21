using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Exceptions;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Domain.Entities.Questionnaires
{
    public class QuestionnaireModuleTemplateTests
    {
        [Fact]
        public void Questionnaire_Section_Set_Throws_Error_For_Null_Or_Empty_Value()
        {
            var questionnaire = new QuestionnaireModuleTemplate();
            var emptySectionsList = new List<SectionTemplate>();

            Assert.Throws<InvalidStructureException>(() => questionnaire.Sections = null);
            Assert.Throws<InvalidStructureException>(() => questionnaire.Sections = emptySectionsList);
        }
    }
}