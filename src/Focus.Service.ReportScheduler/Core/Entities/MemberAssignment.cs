using Focus.Service.ReportScheduler.Core.Enums;

namespace Focus.Service.ReportScheduler.Core.Entities
{
    public class MemberAssignment
    {
        public MemberAssignment(string user, ReportAccessRole role)
        {
            User = user;
            Role = role;
        }

        public string User { get; private set; }
        public ReportAccessRole Role { get; private set; }
    }
}