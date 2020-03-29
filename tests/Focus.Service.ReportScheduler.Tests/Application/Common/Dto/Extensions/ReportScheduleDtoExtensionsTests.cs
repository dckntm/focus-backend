using System;
using Focus.Service.ReportScheduler.Application.Common.Dto;
using Focus.Service.ReportScheduler.Application.Common.Dto.Extensions;
using Focus.Service.ReportScheduler.Domain.Entities;
using Xunit;

namespace Focus.Service.ReportScheduler.Tests.Application.Common.Dto.Extensions
{
    public class ReportScheduleDtoExtensionsTests
    {
        [Fact]
        public void Dto_To_Entity_Mapping_Test()
        {
            var scheduleId = Guid.NewGuid().ToString();
            var reportTemplateId = Guid.NewGuid().ToString();

            var dto = new ReportScheduleDto
            {
                Id = scheduleId,
                ReportTemplate = reportTemplateId,
                Organizations = new OrganizationAssignment[] { },
                DeadlinePeriod = "0.1.0",
                EmissionPeriod = "0.1.0",
                EmissionStart = "01.01.2019",
                EmissionEnd = "01.01.2020"
            };

            var expectedEntity = new ReportSchedule
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
                EmissionStart = new DateTime(2019, 1, 1).ToUniversalTime(),
                EmissionEnd = new DateTime(2020, 1, 1).ToUniversalTime(),
            };

            var entity = dto.AsEntity();

            Assert.Equal(expectedEntity.Id, entity.Id);
            Assert.Equal(expectedEntity.ReportTemplate, entity.ReportTemplate);
            Assert.Equal(expectedEntity.DeadlinePeriod, entity.DeadlinePeriod);
            Assert.Equal(expectedEntity.EmissionPeriod, entity.EmissionPeriod);
            Assert.Equal(expectedEntity.EmissionStart, entity.EmissionStart);
            Assert.Equal(expectedEntity.EmissionEnd, entity.EmissionEnd);
        }

        [Fact]
        public void Entity_To_Dto_Mapping_Test()
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

            var expectedDto = new ReportScheduleDto
            {
                Id = scheduleId,
                ReportTemplate = reportTemplateId,
                Organizations = new OrganizationAssignment[] { },
                DeadlinePeriod = "0.1.0",
                EmissionPeriod = "0.1.0",
                EmissionStart = "01.01.2019",
                EmissionEnd = "01.01.2020"
            };

            var dto = entity.AsDto();

            Assert.Equal(expectedDto.Id, dto.Id);
            Assert.Equal(expectedDto.ReportTemplate, dto.ReportTemplate);
            Assert.Equal(expectedDto.DeadlinePeriod, dto.DeadlinePeriod);
            Assert.Equal(expectedDto.EmissionPeriod, dto.EmissionPeriod);
            Assert.Equal(expectedDto.EmissionStart, dto.EmissionStart);
            Assert.Equal(expectedDto.EmissionEnd, dto.EmissionEnd);
        }
    }
}