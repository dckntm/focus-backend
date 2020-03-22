using Focus.Service.ReportConstructor.Domain.Entities;

namespace Focus.Service.ReportConstructor.Application.Common.Dto.Extensions
{
    public static class ReportTemplateInfoDtoExtensions
    {
        public static ReportTemplateInfoDto AsInfoDto(this ReportTemplate report)
            => new ReportTemplateInfoDto
            {
                Id = report.Id,
                Title = report.Title
            };
    }
}