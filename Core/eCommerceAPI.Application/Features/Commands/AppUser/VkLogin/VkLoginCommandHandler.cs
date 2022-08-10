using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.VkLogin
{
    public class VkLoginCommandHandler : IRequestHandler<VkLoginCommandRequest, VkLoginCommandResponse>
    {
        readonly IAuthService _authService;
        readonly IMapper _mapper;

        public VkLoginCommandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<VkLoginCommandResponse> Handle(VkLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.VkLoginAsync(request.AuthToken,request.Id, request.Provider);

            return _mapper.Map<VkLoginCommandResponse>(token);
        }
    }
}
