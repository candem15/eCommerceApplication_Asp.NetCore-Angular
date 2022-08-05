using eCommerceAPI.Application.Abstractions.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using eCommerceAPI.Application.Exceptions;
using eCommerceAPI.Application.Dtos;

namespace eCommerceAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FacebookLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            JObject accessTokenResponse = JObject.Parse(await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id={_configuration.GetSection("ExternalLogin")["Facebook:ClientId"]}&client_secret={_configuration.GetSection("ExternalLogin")["Facebook:ClientSecret"]}&grant_type=client_credentials"));

            JObject userAccessTokenValidation = JObject.Parse(await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={accessTokenResponse["access_token"]}"));

            JObject validationData = (JObject)userAccessTokenValidation["data"];

            if ((bool)validationData.GetValue("is_valid") == true)
            {
                JObject userInfoResponse = JObject.Parse(await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}"));

                var info = new UserLoginInfo(request.Provider, (string)validationData.GetValue("user_id"), request.Provider);

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
                            Email = userInfoResponse["email"].ToString(),
                            UserName = userInfoResponse["email"].ToString(),
                            Name = userInfoResponse["name"].ToString()
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
            return new()
            {
                Token = token
            };
        }
    }
}
