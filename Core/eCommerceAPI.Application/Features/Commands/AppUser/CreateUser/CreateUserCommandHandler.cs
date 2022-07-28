using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using eCommerceAPI.Application.Exceptions;

namespace eCommerceAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly IMapper _mapper;
        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser createUser = _mapper.Map<Domain.Entities.Identity.AppUser>(request);
            createUser.Id = Guid.NewGuid().ToString();
            IdentityResult result = await _userManager.CreateAsync(createUser);
            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "User created successfully!";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }
    }
}
