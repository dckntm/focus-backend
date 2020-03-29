using Focus.Service.ReportScheduler.Domain.Enums;

namespace Focus.Service.ReportScheduler.Domain.Entities
{
    public class Assignment
    {
        public string UserId { get; set; }
        public ReportAccessRole Role { get; set; }
    }
}