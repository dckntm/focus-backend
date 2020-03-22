using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Focus.Service.ReportConstructor.Application.Common.Dto.Extensions;
using Focus.Service.ReportConstructor.Application.Common.Interface;
using Focus.Service.ReportConstructor.Application.Queries;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using Moq;
using Xunit;
using static Focus.Service.ReportConstructor.Application.Queries.GetReportTemplate;

namespace Focus.Service.ReportConstructor.Tests.Application.Queries
{
    public class GetReportTemplateTests
    {
        [Fact]
        public async Task Get_Report_Template_Returns_Expected()
        {
            var query = new GetReportTemplate("1");

            var repositoryMock = new Mock<IReportTemplateRepository>();

            var template = new ReportTemplate()
            {
                Id = "id",
                Title = "title",
                Modules = new List<ModuleTemplate>()
                {
                    new QuestionnaireModuleTemplate()
                    {
                        Sections = new List<SectionTemplate>()
                        {
                            new SectionTemplate()
                            {
                                Questions = new List<QuestionTemplate>()
                                {
                                    new QuestionTemplate(),
                                    new QuestionTemplate()
                                }
                            }
                        }
                    }
                }

            };

            var dto = template.AsDto();

            repositoryMock
                .Setup(x => x.GetReportTemplateAsync("1"))
                .Returns(Task.FromResult(template));

            var queryHandler = new GetReportTemplateHandler(repositoryMock.Object);

            var result = await queryHandler.HandleAsync(query);

            Assert.Equal(dto.Id, result.Id);
            Assert.Equal(dto.Title, result.Title);

            // TODO optimize check

            foreach (var questionnaire in dto.Questionnaires)
                Assert.Contains(questionnaire, result.Questionnaires);

            foreach (var questionnaire in result.Questionnaires)
                Assert.Contains(questionnaire, dto.Questionnaires);

            foreach (var table in dto.Tables)
                Assert.Contains(table, result.Tables);

            foreach (var table in result.Tables)
                Assert.Contains(table, dto.Tables);
        }

        [Fact]
        public async Task Get_Report_Template_Throws_Exception()
        {
            var query = new GetReportTemplate("");

            var mockRepository = new Mock<IReportTemplateRepository>();

            mockRepository
                .Setup(x => x.GetReportTemplateAsync(""))
                .ThrowsAsync(new Exception());

            var queryHandler = new GetReportTemplateHandler(mockRepository.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await queryHandler.HandleAsync(query));
        }
    }
}