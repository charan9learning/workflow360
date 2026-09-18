using WorkFlow360.Application.Common.Messaging;

namespace WorkFlow360.Application.Authentication.Login
{
    public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
}
