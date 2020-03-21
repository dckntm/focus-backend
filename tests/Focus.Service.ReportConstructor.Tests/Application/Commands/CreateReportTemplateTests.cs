using System;
using System.Threading.Tasks;
using Focus.Service.ReportConstructor.Application.Commands;
using Focus.Service.ReportConstructor.Application.Common.Interface;
using Focus.Service.ReportConstructor.Domain.Entities;
using Moq;
using Xunit;
using static Focus.Service.ReportConstructor.Application.Commands.CreateReportTemplate;

namespace Focus.Service.ReportConstructor.Tests.Application.Commands
{
    public class CreateReportTemplateTests
    {
        [Fact]
        public async Task Create_NULL_Report_Template_Throws_Exception()
        {
            var command = new CreateReportTemplate(null);

            var repoMock = new Mock<IReportTemplateRepository>();

            repoMock
                .Setup(x => x.CreateReportTemplateAsync(It.IsAny<ReportTemplate>()))
                .Returns(Task.FromResult(""));

            var commandHandler = new CreateReportTemplateHandler(repoMock.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await commandHandler.HandleAsync(command));
        }
    }
}