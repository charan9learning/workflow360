using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkFlow360.Api.Contracts.Authentication;
using WorkFlow360.Application.Authentication.Login;
using WorkFlow360.Application.Authentication.Register;

namespace WorkFlow360.Api.Controllers
{
    [Route("api/auth")]
    public sealed class AuthController : ApiController
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(request.Email, request.FirstName, request.LastName, request.Password);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure) return Problem(result.Error);

            return Created($"/api/users/{result.Value.Id}", result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure) return Problem(result.Error);

            return Ok(result.Value);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = User.FindFirstValue(ClaimTypes.Name),
                Role = User.FindFirstValue(ClaimTypes.Role),
                IsAuthenticated = User.Identity?.IsAuthenticated
            });
        }
    }
}
