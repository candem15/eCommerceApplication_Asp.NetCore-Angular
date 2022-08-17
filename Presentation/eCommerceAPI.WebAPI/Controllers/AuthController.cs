using eCommerceAPI.Application.Features.Commands.AppUser.FacebookLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using eCommerceAPI.Application.Features.Commands.AppUser.MicrosoftLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.TwitterLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.VkLogin;
using eCommerceAPI.Application.Features.Queries.Authentication.Twitter.GetRequestToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            FacebookLoginCommandResponse response = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("vk-login")]
        public async Task<IActionResult> VkLogin(VkLoginCommandRequest vkLoginCommandRequest)
        {
            VkLoginCommandResponse response = await _mediator.Send(vkLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("microsoft-login")]
        public async Task<IActionResult> MicrosoftLogin(MicrosoftLoginCommandRequest microsoftLoginCommandRequest)
        {
            MicrosoftLoginCommandResponse response = await _mediator.Send(microsoftLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("twitter-login")]
        public async Task<IActionResult> TwitterLogin(TwitterLoginCommandRequest twitterLoginCommandRequest)
        {
            TwitterLoginCommandResponse response = await _mediator.Send(twitterLoginCommandRequest);
            return Ok(response);
        }

        [HttpGet("get-twitter-request-token")]
        public async Task<IActionResult> GetTwitterRequestToken()
        {
            GetRequestTokenQueryResponse response = await _mediator.Send(new GetRequestTokenQueryRequest());
            return Ok(response);
        }

        [HttpPost("refresh-token-login")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody]RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }
    }
}
