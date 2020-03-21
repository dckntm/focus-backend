using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Exceptions;
using Xunit;

namespace Focus.Service.ReportConstructor.Tests.Domain.Entities
{
    public class ReportTemplateTests
    {
        [Fact]
        public void Report_Template_Modules_Set_Throws_Error_For_Null_Or_Empty_Value()
        {
            var reportTemplate = new ReportTemplate();
            var emptyModulesList = new List<ModuleTemplate>();

            Assert.Throws<InvalidStructureException>(() => reportTemplate.Modules = null);
            Assert.Throws<InvalidStructureException>(() => reportTemplate.Modules = emptyModulesList);
        }
    }
}