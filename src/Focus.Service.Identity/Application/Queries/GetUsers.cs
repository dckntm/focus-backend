using Focus.Application.Common.Abstract;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Focus.Service.Identity.Application.Services;
using System;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetUsers : IRequest<Result> { }

    public class GetUsersHandler :
        IRequestHandler<GetUsers, Result>
    {
        private readonly IIdentityRepository _repository;

        public GetUsersHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            try
            {
                return Result.Success(await _repository.GetUsersAsync());
            }
            catch (Exception ex)
            {
                return Result.Fail(ex);
            }
        }
    }
}