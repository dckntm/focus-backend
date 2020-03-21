using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Focus.Service.ReportConstructor.Domain.Exceptions;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Domain.Entities.Questionnaires
{
    public class SectionTemplateTests
    {
        [Fact]
        public void Section_Questions_Set_Throws_Error_For_Null_Or_Empty_Value()
        {
            var section = new SectionTemplate();
            var emptyQuestionList = new List<QuestionTemplate>();

            Assert.Throws<InvalidStructureException>(() => section.Questions = null);
            Assert.Throws<InvalidStructureException>(() => section.Questions = emptyQuestionList);
        }
    }
}