using Focus.Service.ReportConstructor.Application.Queries;
using static Focus.Service.ReportConstructor.Application.Queries.GetReportTemplateInfos;
using Xunit;
using Moq;
using Focus.Service.ReportConstructor.Application.Common.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using Focus.Service.ReportConstructor.Domain.Entities;
using Focus.Service.ReportConstructor.Domain.Common.Abstract;
using Focus.Service.ReportConstructor.Domain.Entities.Table;
using Focus.Service.ReportConstructor.Domain.Entities.Questionnaires;
using System.Linq;
using Focus.Service.ReportConstructor.Application.Common.Dto.Extensions;

namespace Focus.Service.ReportConstructor.Tests.Application.Queries
{
    public class GetReportTemplateInfosTests
    {
        [Fact]
        public async Task Get_Report_Template_Infos_Returns_Expected()
        {
            var query = new GetReportTemplateInfos();

            var mockRepo = new Mock<IReportTemplateRepository>();

            var entities = new List<ReportTemplate>()
            {
                new ReportTemplate()
                {
                    Id = "1",
                    Title = "RTE 1",
                    Modules = new List<ModuleTemplate>()
                    {
                        new TableModuleTemplate()
                        {
                            Cells = new List<CellTemplate>()
                            {
                                new CellTemplate(),
                                new CellTemplate(),
                                new CellTemplate()
                            }
                        },
                        new TableModuleTemplate()
                        {
                            Cells = new List<CellTemplate>()
                            {
                                new CellTemplate(),
                                new CellTemplate()
                            }
                        },
                        new QuestionnaireModuleTemplate() {
                            Sections = new List<SectionTemplate>()
                            {
                                new SectionTemplate(){
                                    Questions = new List<QuestionTemplate>()
                                    {
                                        new QuestionTemplate(),
                                        new QuestionTemplate()
                                    }
                                }
                            }
                        }
                    }
                }
            }.AsQueryable();

            mockRepo
                .Setup(x => x.GetReportTemplatesAsync())
                .ReturnsAsync(entities);

            var queryHandler = new GetReportTemplateInfosHandler(mockRepo.Object);

            var result = await queryHandler.HandleAsync(query);

            var expected = entities
                .Select(x => x.AsInfoDto());

            foreach (var dto in expected)
                Assert.True(result.Any(x => x.Id == dto.Id && x.Title == dto.Title));
        }
    }
}