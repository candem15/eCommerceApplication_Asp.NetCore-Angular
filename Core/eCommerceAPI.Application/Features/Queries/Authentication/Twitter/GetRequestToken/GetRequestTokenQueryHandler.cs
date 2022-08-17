using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Queries.Authentication.Twitter.GetRequestToken
{
    public class GetRequestTokenQueryHandler : IRequestHandler<GetRequestTokenQueryRequest, GetRequestTokenQueryResponse>
    {
        readonly IAuthService _authService;
        readonly IMapper _mapper;
        public GetRequestTokenQueryHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<GetRequestTokenQueryResponse> Handle(GetRequestTokenQueryRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetTwitterRequestTokenAsync();
            return _mapper.Map<GetRequestTokenQueryResponse>(token);
        }
    }
}
