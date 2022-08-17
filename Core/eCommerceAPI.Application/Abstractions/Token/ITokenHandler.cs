using eCommerceAPI.Domain.Entities.Identity;

namespace eCommerceAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        Dtos.Token CreateAccessToken(int seconds,AppUser user);
        string CreateRefreshToken();
    }
}
