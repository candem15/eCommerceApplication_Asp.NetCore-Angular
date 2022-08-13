using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.TwitterLogin
{
    public class TwitterLoginCommandRequest : IRequest<TwitterLoginCommandResponse>
    {
        public string OauthToken { get; set; }
        public string OauthVerifier { get; set; }
    }
}
