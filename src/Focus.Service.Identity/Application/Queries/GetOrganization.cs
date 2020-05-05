using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using MediatR;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetOrganization : IRequest<Result>
    {
        public GetOrganization(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }

    public class GetOrganizationHandler : IRequestHandler<GetOrganization, Result>
    {
        private readonly IIdentityRepository _repository;

        public GetOrganizationHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetOrganization request, CancellationToken cancellationToken)
        {
            try
            {
                var organization = await _repository.GetOrganizationAsync(request.Id);

                return Result.Success(organization);
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}