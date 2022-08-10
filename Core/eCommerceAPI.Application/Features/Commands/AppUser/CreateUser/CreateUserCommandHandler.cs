using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using eCommerceAPI.Application.Exceptions;
using eCommerceAPI.Application.Abstractions.Services;

namespace eCommerceAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;
        readonly IMapper _mapper;
        public CreateUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            Application.Dtos.User.CreateUser createUser = _mapper.Map<Application.Dtos.User.CreateUser>(request);
            CreateUserCommandResponse response = _mapper.Map<CreateUserCommandResponse>(await _userService.CreateAsync(createUser));
            return response;
        }
    }
}
