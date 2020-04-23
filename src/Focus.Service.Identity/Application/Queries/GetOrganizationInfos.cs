using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Dto;
using Focus.Service.Identity.Application.Services;
using MediatR;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetOrganizationInfos : IRequest<RequestResult<IEnumerable<OrganizationInfoDto>>>
    {
    }

    public class GetOrganizationInfosHandler :
        IRequestHandler<GetOrganizationInfos, RequestResult<IEnumerable<OrganizationInfoDto>>>
    {
        private readonly IIdentityRepository _repository;

        public GetOrganizationInfosHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<IEnumerable<OrganizationInfoDto>>> Handle(GetOrganizationInfos request, CancellationToken cancellationToken)
        {
            try
            {
                var orgs = await _repository.GetOrganizationsAsync();

                return RequestResult
                    .Successfull(orgs.Select(x => new OrganizationInfoDto()
                    {
                        Id = x.Id,
                        Title = x.TItle,
                        IsHead = x.IsHead
                    }));
            }
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<OrganizationInfoDto>>
                    .Failed(ex);
            }
        }
    }
}