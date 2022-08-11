using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.MicrosoftLogin
{
    public class MicrosoftLoginCommandHandler : IRequestHandler<MicrosoftLoginCommandRequest, MicrosoftLoginCommandResponse>
    {
        readonly IMapper _mapper;
        readonly IAuthService _authService;

        public MicrosoftLoginCommandHandler(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<MicrosoftLoginCommandResponse> Handle(MicrosoftLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.MicrosoftLoginAsync(request.AuthToken, request.Provider);
            return _mapper.Map<MicrosoftLoginCommandResponse>(token);
        }
    }
}
