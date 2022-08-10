using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.VkLogin
{
    public class VkLoginCommandRequest : IRequest<VkLoginCommandResponse>
    {
        public string AuthToken { get; set; }
        public int Id { get; set; }
        public string Provider { get; set; }
    }
}
