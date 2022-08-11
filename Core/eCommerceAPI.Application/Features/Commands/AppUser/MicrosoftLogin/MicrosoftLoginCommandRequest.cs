using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.MicrosoftLogin
{
    public class MicrosoftLoginCommandRequest : IRequest<MicrosoftLoginCommandResponse>
    {
        public string AuthToken { get; set; }
        public string Provider { get; set; }
    }
}
