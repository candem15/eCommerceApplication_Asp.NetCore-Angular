using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.TwitterLogin
{
    public class TwitterLoginCommandHandler : IRequestHandler<TwitterLoginCommandRequest, TwitterLoginCommandResponse>
    {
        readonly IMapper _mapper;
        readonly IAuthService _authService;

        public TwitterLoginCommandHandler(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<TwitterLoginCommandResponse> Handle(TwitterLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.TwitterLoginAsync(request.OauthToken, request.OauthVerifier);
            return _mapper.Map<TwitterLoginCommandResponse>(token);
        }
    }
}
