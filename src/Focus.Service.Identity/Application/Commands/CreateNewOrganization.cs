using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using MediatR;

namespace Focus.Service.Identity.Application.Commands
{
    public class CreateNewOrganization : IRequest<RequestResult<string>>
    {
        public Organization Organization { get; private set; }

        public CreateNewOrganization(Organization organization)
        {
            Organization = organization;
        }
    }

    public class CreateNewOrganizationHandler : IRequestHandler<CreateNewOrganization, RequestResult<string>>
    {
        private readonly IIdentityRepository _repository;

        public CreateNewOrganizationHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<string>> Handle(CreateNewOrganization request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Organization.IsHead)
                    throw new Exception("APPLICATION Can't create new Head Organization");

                var id = await _repository.CreateNewOrganizationAsync(request.Organization);

                return RequestResult
                    .Successfull(id);
            }
            catch (Exception ex)
            {
                return RequestResult<string>
                    .Failed(ex);
            }
        }
    }
}