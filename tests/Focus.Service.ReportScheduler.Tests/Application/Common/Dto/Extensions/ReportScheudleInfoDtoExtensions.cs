using System;
using Focus.Service.ReportScheduler.Application.Common.Dto;
using Focus.Service.ReportScheduler.Application.Common.Dto.Extensions;
using Focus.Service.ReportScheduler.Domain.Entities;
using Xunit;

namespace Focus.Service.ReportScheduler.Tests.Application.Common.Dto.Extensions
{
    public class ReportScheduleInfoDtoTests
    {
        [Fact]
        public void Entity_To_Info_Dto_Mapping_Test()
        {
            var scheduleId = Guid.NewGuid().ToString();
            var reportTemplateId = Guid.NewGuid().ToString();


            var entity = new ReportSchedule
            {
                Id = scheduleId,
                ReportTemplate = reportTemplateId,
                Organizations = new OrganizationAssignment[] { },
                DeadlinePeriod = new Period
                {
                    Days = 0,
                    Months = 1,
                    Years = 0
                },
                EmissionPeriod = new Period
                {
                    Days = 0,
                    Months = 1,
                    Years = 0
                },
                EmissionStart = new DateTime(2019, 1, 1),
                EmissionEnd = new DateTime(2020, 1, 1),
            };

            var expectedDto = new ReportScheduleInfoDto
            {
                Id = scheduleId,
                ReportTemplate = reportTemplateId,
                AssignedOrganizations = new string[] {},
                EmissionPeriod = "01.01.2019-01.01.2020",
                DeadlinePeriod = "0.1.0"
            };

            var dto = entity.AsInfoDto();

            Assert.Equal(expectedDto.Id, dto.Id);
            Assert.Equal(expectedDto.ReportTemplate, dto.ReportTemplate);
            Assert.Equal(expectedDto.EmissionPeriod, dto.EmissionPeriod);
            Assert.Equal(expectedDto.DeadlinePeriod, dto.DeadlinePeriod);
        }
    }
}