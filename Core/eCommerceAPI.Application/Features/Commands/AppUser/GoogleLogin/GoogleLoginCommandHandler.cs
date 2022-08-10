using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;

namespace eCommerceAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GoogleLoginCommandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GoogleLoginAsync(request.IdToken, request.Provider);

            return _mapper.Map<GoogleLoginCommandResponse>(token);
        }
    }
}

