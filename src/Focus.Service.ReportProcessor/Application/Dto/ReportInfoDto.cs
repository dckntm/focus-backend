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
}