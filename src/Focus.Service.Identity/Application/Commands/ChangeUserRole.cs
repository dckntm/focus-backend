using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using Focus.Service.Identity.Core.Enums;
using MediatR;

namespace Focus.Service.Identity.Application.Commands
{
    public class ChangeUserRole : IRequest<Result>
    {
        public string Username { get; private set; }
        public string NewRole { get; set; }

        public ChangeUserRole(string username, string newRole)
        {
            Username = username;
            NewRole = newRole;
        }
    }

    public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRole, Result>
    {
        private readonly IIdentityRepository _repository;

        public ChangeUserRoleHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(ChangeUserRole request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetUserAsync(request.Username);

                if (user.Role.Value() != request.NewRole)
                {
                    user.Role = request.NewRole switch
                    {
                        "HOA" => UserRole.HeadOrganizationAdmin,
                        "COA" => UserRole.ChildOrganizationAdmin,
                        "HOM" => UserRole.HeadOrganizationMember,
                        "COM" => UserRole.ChildOrganizationMember,
                        _ => throw new Exception($"APPLICATION No such User Role of value {request.NewRole}")
                    };

                    if (user.Role is UserRole.HeadOrganizationAdmin)
                    {
                        throw new Exception("APPLICATION Can't change role to Head Organization Administrator");
                    }

                    if (user.Role is UserRole.ChildOrganizationAdmin)
                    {
                        IQueryable<User> organizationMembers = await _repository.GetOrganizationMembers(user.Organization);

                        var oldAdmin = organizationMembers
                            .First(x => x.Role == UserRole.ChildOrganizationAdmin);

                        await _repository.ChangeUserRole(oldAdmin.Username, UserRole.ChildOrganizationMember.Value());
                    }

                    await _repository.ChangeUserRole(user.Username, request.NewRole);

                    return Result.Success($"Switched to role {request.NewRole}");
                }

                return Result.Success($"User already has {request.NewRole}");
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}