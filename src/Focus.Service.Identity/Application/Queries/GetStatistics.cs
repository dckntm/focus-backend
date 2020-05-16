using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetStatistics : IRequest<Result> { }

    public class GetStatisticsHandler : IRequestHandler<GetStatistics, Result>
    {
        private readonly IIdentityRepository _repository;
        private readonly ILogger<GetStatisticsHandler> _logger;

        public GetStatisticsHandler(IIdentityRepository repository, ILogger<GetStatisticsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result> Handle(GetStatistics request, CancellationToken cancellationToken)
        {
            try
            {
                var organizations = await _repository.GetOrganizationsAsync();
                var users = await _repository.GetUsersAsync();

                return Result.Success(new
                {
                    TotalUsers = users.Count(),
                    TotalOrganizations = organizations.Count()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "In Get Statistics Handler");
                return Result.Fail(e);
            }
        }
    }
}