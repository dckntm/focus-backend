using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using Focus.Service.Identity.Core.Entities;
using MediatR;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetUser : IRequest<RequestResult<User>>
    {
        public GetUser(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }

    public class GetUserHandler : IRequestHandler<GetUser, RequestResult<User>>
    {
        private readonly IIdentityRepository _repository;

        public GetUserHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<User>> Handle(GetUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetUserAsync(request.Username);

                return RequestResult
                    .Successfull(user);
            }
            catch (Exception ex)
            {
                return RequestResult<User>
                    .Failed(ex);
            }
        }
    }
}