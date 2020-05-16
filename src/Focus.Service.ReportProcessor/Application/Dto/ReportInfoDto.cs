using Focus.Service.ReportProcessor.Entities;
using Focus.Service.ReportProcessor.Enums;

namespace Focus.Service.ReportProcessor.Application.Dto
{
    public class ReportInfoDto
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string AssignedOrganizationId { get; set; }
        public string ReportStatus { get; set; }
        public string Deadline { get; set; }
    }

    public static class ReportInfoDtoExtensions
    {
        public static ReportInfoDto AsInfoDto(this Report report)
            => new ReportInfoDto
            {
                Title = report.Title,
                Id = report.Id,
                AssignedOrganizationId = report.AssignedOrganizationId,
                ReportStatus = report.Status switch
                {
                    ReportStatus.InProgress => "InProgress",
                    ReportStatus.Overdue => "Overdue",
                    ReportStatus.Passed => "Passed",
                    _ => ""
                },
                Deadline = report.Deadline.ToString("dd.MM.yyyy")
            };
    }
}