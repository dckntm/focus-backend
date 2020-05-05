using Focus.Service.Identity.Core.Enums;

namespace Focus.Service.Identity.Application.Services
{
    public interface ISecurityTokenGenerator
    {
        string Generate(string username, UserRole role, string orgId);
    }
}