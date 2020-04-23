using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Focus.Service.Identity.Core.Entities;

namespace Focus.Service.Identity.Application.Services
{
    public interface IIdentityRepository
    {
        Task<User> GetUserAsync(string username);
        Task<string> CreateNewOrganizationAsync(Organization organization);
        Task<Organization> GetOrganizationAsync(string id);
        Task<bool> CreateNewUserAsync(string name, string surname, string patronymic, string username, string password, string userRole, string organizationId);
        Task<IQueryable<User>> GetOrganizationMembers(string organization);
        Task ChangeUserRole(string username, string newRole);
        Task<IEnumerable<Organization>> GetOrganizationsAsync();
        Task<IEnumerable<User>> GetUsersAsync();

    }
}