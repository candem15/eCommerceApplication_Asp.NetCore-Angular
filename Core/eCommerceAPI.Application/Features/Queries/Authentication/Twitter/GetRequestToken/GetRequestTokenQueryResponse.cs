namespace eCommerceAPI.Application.Features.Queries.Authentication.Twitter.GetRequestToken
{
    public class GetRequestTokenQueryResponse
    {
        public string oauth_token { get; set; }
        public string oauth_token_secret { get; set; }
        public string oauth_callback_confirmed { get; set; }
    }
}