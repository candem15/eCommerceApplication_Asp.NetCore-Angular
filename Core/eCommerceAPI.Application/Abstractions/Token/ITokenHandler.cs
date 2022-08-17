namespace eCommerceAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        Dtos.Token CreateAccessToken(int seconds);
        string CreateRefreshToken();
    }
}
