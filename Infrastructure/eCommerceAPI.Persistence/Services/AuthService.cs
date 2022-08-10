using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.Dtos;
using eCommerceAPI.Application.Exceptions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

        public AuthService(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory, IConfiguration configuration, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _signInManager = signInManager;
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

                bool result = user != null;

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userInfoResponse["email"].ToString());
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfoResponse["email"]?.ToString(),
                            UserName = userInfoResponse["email"]?.ToString(),
                            Name = userInfoResponse["name"]?.ToString()
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }

                if (result)
                    await _userManager.AddLoginAsync(user, info);
                else
                    throw new ExternalLoginFailedException();
            }
            Token token = _tokenHandler.CreateAccessToken(10);
            return token;
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

            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        Name = payload.Name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                throw new ExternalLoginFailedException();

            Token token = _tokenHandler.CreateAccessToken(10);

            return token;
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
                Token token = _tokenHandler.CreateAccessToken(10);

                return token;
            }

            throw new AuthenticationFailedException();
        }

        public Task TwitterLoginAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Token> VkLoginAsync(string authToken, int id, string provider)
        {
            JObject userInfoResponse = JObject.Parse(await _httpClient.GetStringAsync($"https://api.vk.com/method/users.get?user_ids={id}&access_token={authToken}&v=6.0"));

            JObject infoResponse = (JObject)userInfoResponse["response"][0];

            var info = new UserLoginInfo(provider, id.ToString(), provider);

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync($"{id}@ecommerce.com");
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = $"{id}@ecommerce.com",
                        UserName = $"{id}@ecommerce.com",
                        Name = infoResponse["first_name"].ToString()+" "+ infoResponse["last_name"]
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                throw new ExternalLoginFailedException();

            Token token = _tokenHandler.CreateAccessToken(10);
            return token;
        }
    }
}
