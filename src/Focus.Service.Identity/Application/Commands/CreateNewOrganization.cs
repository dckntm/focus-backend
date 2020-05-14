using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using MediatR;

namespace Focus.Service.Identity.Application.Commands
{
    public class CreateNewOrganization : IRequest<Result>
    {
        public Organization Organization { get; private set; }

        public CreateNewOrganization(Organization organization)
        {
            Organization = organization;
        }
    }

    public class CreateNewOrganizationHandler : IRequestHandler<CreateNewOrganization, Result>
    {
        private readonly IIdentityRepository _repository;

        public CreateNewOrganizationHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(CreateNewOrganization request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Organization.IsHead)
                    throw new Exception("APPLICATION Can't create new Head Organization");

                var id = await _repository.CreateNewOrganizationAsync(request.Organization);

                return Result.Success(id);
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}