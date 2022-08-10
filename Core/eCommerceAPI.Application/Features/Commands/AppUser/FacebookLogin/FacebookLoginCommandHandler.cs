using MediatR;
using eCommerceAPI.Application.Abstractions.Services;
using AutoMapper;

namespace eCommerceAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public FacebookLoginCommandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.FacebookLoginAsync(request.AuthToken, request.Provider);

            return _mapper.Map<FacebookLoginCommandResponse>(token);
        }
    }
}
