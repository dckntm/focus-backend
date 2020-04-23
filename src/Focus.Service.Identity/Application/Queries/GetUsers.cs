using Focus.Application.Common.Abstract;
using Focus.Service.Identity.Core.Entities;
using System.Collections.Generic;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Focus.Service.Identity.Application.Services;
using System;

namespace Focus.Service.Identity.Application.Queries
{
    public class GetUsers : IRequest<RequestResult<IEnumerable<User>>>
    {

    }

    public class GetUsersHandler :
        IRequestHandler<GetUsers, RequestResult<IEnumerable<User>>>
    {
        private readonly IIdentityRepository _repository;

        public GetUsersHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<IEnumerable<User>>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            try
            {
                return RequestResult
                    .Successfull(await _repository.GetUsersAsync());
            }
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<User>>
                    .Failed(ex);
            }
        }
    }
}