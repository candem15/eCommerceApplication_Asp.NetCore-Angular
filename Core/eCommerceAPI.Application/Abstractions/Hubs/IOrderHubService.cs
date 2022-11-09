using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Abstractions.Hubs
{
    public interface IOrderHubService
    {
        Task OrderCreatedMessageAsync(string message);
    }
}
