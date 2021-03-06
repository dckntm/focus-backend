using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Application.Dto;
using Focus.Service.Identity.Application.Services;
using MediatR;

namespace Focus.Service.Identity.Application.Commands
{
    public class CreateNewUser : IRequest<Result>
    {
        public CreateNewUser(NewUserDto newUserData)
        {
            NewUserData = newUserData;
        }

        public NewUserDto NewUserData { get; private set; }
    }

    public class CreateNewUserHandler : IRequestHandler<CreateNewUser, Result>
    {
        private readonly IIdentityRepository _repository;
        private readonly IPasswordGenerator _password;
        public CreateNewUserHandler(
            IIdentityRepository repository,
            IPasswordGenerator password)
        {
            _password = password;
            _repository = repository;
        }

        public async Task<Result> Handle(CreateNewUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.NewUserData;

                var username = BuildUsername(user);

                var password = _password.Generate(
                    useLowercase: true,
                    useUppercase: false,
                    useNumbers: true,
                    useSpecial: false,
                    passwordSize: 12);

                // TODO: think how to make sure that username is unique

                if (!await _repository.CreateNewUserAsync(
                    user.Name,
                    user.Surname,
                    user.Patronymic,
                    username,
                    password,
                    user.UserRole,
                    user.OrganizationId))
                {
                    var random = new Random();
                    username += random.Next(100).ToString();

                    if (!await _repository.CreateNewUserAsync(
                        user.Name,
                        user.Surname,
                        user.Patronymic,
                        username,
                        password,
                        user.UserRole,
                        user.OrganizationId))

                        return Result
                            .Fail(message: $"APPLICATION Can't create user with username: {username}. Try again");
                }

                return Result.Success((username, password));
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }

        private string BuildUsername(NewUserDto user)
            => user.Surname.First().ToString().ToUpper() +
                user.Surname.Substring(1).ToLower() +
                user.Name.First().ToString().ToUpper() +
                user.Patronymic.First().ToString().ToUpper();

    }
}