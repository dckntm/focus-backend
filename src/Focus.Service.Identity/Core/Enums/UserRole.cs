namespace Focus.Service.Identity.Core.Enums
{
    public enum UserRole
    {
        HeadOrganizationAdmin,
        ChildOrganizationAdmin,
        HeadOrganizationMember,
        ChildOrganizationMember
    }

    public static class UserRoleExtensions
    {
        public static string Value(this UserRole role)
        {
            return role switch
            {
                UserRole.HeadOrganizationAdmin => "HOA",
                UserRole.ChildOrganizationAdmin => "COA",
                UserRole.HeadOrganizationMember => "HOM",
                UserRole.ChildOrganizationMember => "COM",
                _ => throw new System.Exception("This can't happen)))"),
            };
        }
    }
}