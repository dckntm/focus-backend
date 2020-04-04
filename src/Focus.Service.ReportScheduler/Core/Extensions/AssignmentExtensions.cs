using Focus.Service.ReportScheduler.Core.Entities;
using Focus.Service.ReportScheduler.Core.Enums;

namespace Focus.Service.ReportScheduler.Core.Extensions
{
    public static class AssignmentExtensions
    {
        public static Assignment DelegateToCOA(this Assignment assignment)
        {
            assignment.IsDelegatedToCOA = true;

            return assignment;
        }

        public static Assignment AssignTo(this Assignment assignment, string userId)
        {
            assignment.Assignees.Add(new MemberAssignment(userId, ReportAccessRole.Assignee));

            return assignment;
        }

        public static Assignment RequestReviewTo(this Assignment assignment, string userId)
        {
            assignment.Assignees.Add(new MemberAssignment(userId, ReportAccessRole.Reviewer));

            return assignment;
        }

        public static Assignment ViewableFor(this Assignment assignment, string userId)
        {
            assignment.Assignees.Add(new MemberAssignment(userId, ReportAccessRole.Reviewer));

            return assignment;
        }
    }
}