using eCommerceAPI.Application.Dtos.User;
using eCommerceAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser user);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenLifetime, int refreshTokenAddMinutes);
    }
}
