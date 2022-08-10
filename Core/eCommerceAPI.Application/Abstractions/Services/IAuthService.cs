using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<Dtos.Token> LoginAsync(string usernameOrEmail, string password);
        Task<Dtos.Token> FacebookLoginAsync(string authToken, string provider);
        Task<Dtos.Token> GoogleLoginAsync(string idToken, string provider);
        Task<Dtos.Token> VkLoginAsync(string authToken, int id, string provider);
        Task TwitterLoginAsync();
    }
}
