using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.Dtos;
using eCommerceAPI.Application.Dtos.Twitter;
using eCommerceAPI.Application.Exceptions;
using eCommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eCommerceAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly IUserService _userService;

        public AuthService(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory, IConfiguration configuration, SignInManager<Domain.Entities.Identity.AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
        }
        public async Task<Token> FacebookLoginAsync(string authToken, string provider)
        {
            JObject accessTokenResponse = JObject.Parse(await _httpClient.GetStringAsync(
               $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLogin:Facebook:ClientId"]}&client_secret={_configuration["ExternalLogin:Facebook:ClientSecret"]}&grant_type=client_credentials"));

            JObject userAccessTokenValidation = JObject.Parse(await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={accessTokenResponse["access_token"]}"));

            JObject validationData = (JObject)userAccessTokenValidation["data"];

            if ((bool)validationData.GetValue("is_valid") == true)
            {
                JObject userInfoResponse = JObject.Parse(await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}"));

                var info = new UserLoginInfo(provider, (string)validationData.GetValue("user_id"), provider);

                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateExternalLoginTokenAsync(user, userInfoResponse["email"].ToString(), userInfoResponse["name"].ToString(), info, 900);
            }
            throw new ExternalLoginFailedException();
        }

        public async Task<Token> GoogleLoginAsync(string idToken, string provider)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLogin:Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo(provider, payload.Subject, provider);

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateExternalLoginTokenAsync(user, payload.Email, payload.Name, info, 900);
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null)
                throw new NotFoundUserException();
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(10, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 900);
                return token;
            }

            throw new AuthenticationFailedException();
        }

        public async Task<Token> MicrosoftLoginAsync(string authToken, string provider)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            JObject userInfoResponse = JObject.Parse(await _httpClient.GetStringAsync("https://graph.microsoft.com/v1.0/me"));
            if (userInfoResponse["error"] != null)
            {
                throw new ExternalLoginFailedException();
            }
            var info = new UserLoginInfo(provider, (string)userInfoResponse["id"], provider);
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateExternalLoginTokenAsync(user, userInfoResponse["userPrincipalName"].ToString(), userInfoResponse["displayName"].ToString(), info, 900);
        }

        public async Task<Token> TwitterLoginAsync(string oauthToken, string oauthVerifier)
        {
            try
            {
                var accessTokenResponse = await _httpClient.GetStringAsync($"https://api.twitter.com/oauth/access_token?oauth_verifier={oauthVerifier}&oauth_token={oauthToken}");
                var collection = HttpUtility.ParseQueryString(accessTokenResponse);
                JObject responseObject = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(collection.AllKeys.ToDictionary(y => y, y => collection[y])));

                string consumerKey = _configuration["ExternalLogin:Twitter:ClientId"];
                string consumerSecret = _configuration["ExternalLogin:Twitter:ClientSecret"];

                _httpClient.DefaultRequestHeaders.Accept.Clear();

                var oauthClient = new OAuthRequest
                {
                    Method = "GET",
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret,
                    RequestUrl = "https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true",
                    Version = "1.0a",
                    Realm = "twitter.com",
                    Token = responseObject["oauth_token"].ToString(),
                    TokenSecret = responseObject["oauth_token_secret"].ToString()
                };
                string auth = oauthClient.GetAuthorizationHeader();

                _httpClient.DefaultRequestHeaders.Add("Authorization", auth);

                JObject userInfoResponse = JObject.Parse(await _httpClient.GetStringAsync("https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true"));

                var info = new UserLoginInfo("TWITTER", (string)userInfoResponse.GetValue("id"), "TWITTER");

                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateExternalLoginTokenAsync(user, userInfoResponse["email"].ToString(), userInfoResponse["name"].ToString(), info, 900);
            }
            catch
            {
                throw new ExternalLoginFailedException();
            }
        }

        public async Task<RequestTokenResponse> GetTwitterRequestTokenAsync()
        {
            var requestTokenResponse = new RequestTokenResponse();
            var consumerKey = _configuration["ExternalLogin:Twitter:ClientId"];
            var consumerSecret = _configuration["ExternalLogin:Twitter:ClientSecret"];
            var callbackUrl = "http://localhost:4200/register";

            _httpClient.DefaultRequestHeaders.Accept.Clear();

            var oauthClient = new OAuthRequest
            {
                Method = "POST",
                Type = OAuthRequestType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                RequestUrl = "https://api.twitter.com/oauth/request_token",
                Version = "1.0a",
                Realm = "twitter.com",
                CallbackUrl = callbackUrl
            };
            string auth = oauthClient.GetAuthorizationHeader();

            _httpClient.DefaultRequestHeaders.Add("Authorization", auth);

            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");

                using (var response = await _httpClient.PostAsync(oauthClient.RequestUrl, content))
                {
                    response.EnsureSuccessStatusCode();

                    var responseString = response.Content.ReadAsStringAsync()
                                               .Result.Split("&");

                    requestTokenResponse = new RequestTokenResponse
                    {
                        oauth_token = responseString[0],
                        oauth_token_secret = responseString[1],
                        oauth_callback_confirmed = responseString[2]
                    };

                }
            }
            catch
            {

                throw new ExternalLoginFailedException();
            }

            return requestTokenResponse;
        }

        public async Task<Token> VkLoginAsync(string authToken, int id, string provider)
        {
            JObject userInfoResponse = JObject.Parse(await _httpClient.GetStringAsync($"https://api.vk.com/method/users.get?user_ids={id}&access_token={authToken}&v=6.0"));

            JObject infoResponse = (JObject)userInfoResponse["response"][0];

            var info = new UserLoginInfo(provider, id.ToString(), provider);

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateExternalLoginTokenAsync(user, $"{id}@ecommerce.com", infoResponse["first_name"].ToString() + " " + infoResponse["last_name"], info, 900);
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenExpirationTime > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15, user);
                await _userService.UpdateRefreshTokenAsync(refreshToken, user, token.Expiration, 900);
                return token;
            }
            throw new NotFoundUserException();
        }
        public async Task<Token> CreateExternalLoginTokenAsync(AppUser user, string mail, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(mail);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = mail,
                        UserName = mail,
                        Name = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                throw new ExternalLoginFailedException();
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
            return token;
        }
    }
}
