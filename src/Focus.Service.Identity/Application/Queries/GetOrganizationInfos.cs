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
    public class GetOrganizationInfos : IRequest<Result> { }

    public class GetOrganizationInfosHandler :
        IRequestHandler<GetOrganizationInfos, Result>
    {
        private readonly IIdentityRepository _repository;

        public GetOrganizationInfosHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetOrganizationInfos request, CancellationToken cancellationToken)
        {
            try
            {
                var orgs = await _repository.GetOrganizationsAsync();

                return Result.Success(orgs
                    .Select(x => new OrganizationInfoDto()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        IsHead = x.IsHead
                    }));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex);
            }
        }
    }
}