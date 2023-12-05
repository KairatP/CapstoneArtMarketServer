using Capstone.Application.DTOs.Account;
using Capstone.Application.Features.Accounts.Commands;
using Capstone.Application.Features.Accounts.Commands.UpdateProfile;
using Capstone.Application.Features.Accounts.Queries.GetProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Capstone.WebApi.Controllers.v1
{
    [Route("/api/v{version:ApiVersion}/account")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : BaseApiController
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<TokenResponse> Login([FromBody] CreateTokenCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("verifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await Mediator.Send(new GetProfileCommand()));
        }

        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
