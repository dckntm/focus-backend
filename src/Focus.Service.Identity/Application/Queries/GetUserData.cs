using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using MediatR;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetUser : IRequest<Result>
    {
        public GetUser(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }

    public class GetUserHandler : IRequestHandler<GetUser, Result>
    {
        private readonly IIdentityRepository _repository;

        public GetUserHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetUserAsync(request.Username);

                return Result.Success(user);
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}