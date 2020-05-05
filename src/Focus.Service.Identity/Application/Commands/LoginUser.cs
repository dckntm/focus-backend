using System;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Services;
using MediatR;

namespace Focus.Service.Identity.Application.Commands
{
    public class LoginUser : IRequest<Result>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public LoginUser(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public class LoginUserHandler : IRequestHandler<LoginUser, Result>
    {
        private readonly IIdentityRepository _repository;
        private readonly ISecurityTokenGenerator _token;

        public LoginUserHandler(IIdentityRepository repository, ISecurityTokenGenerator token)
        {
            _repository = repository;
            _token = token;
        }

        public async Task<Result> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetUserAsync(request.Username);

                if (!(user is null) && user.Password == request.Password)
                {
                    var token = _token.Generate(user.Username, user.Role, user.Organization);

                    return Result.Success(token);
                }

                return Result.Fail(message: $"APPLICATION: Can't generate token for invalid ({request.Username}, {request.Password}) pair");
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}