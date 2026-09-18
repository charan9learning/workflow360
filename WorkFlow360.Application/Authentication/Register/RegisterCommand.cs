using WorkFlow360.Application.Common.Messaging;

namespace WorkFlow360.Application.Authentication.Register
{
    public sealed record RegisterCommand(string Email, string FirstName, string LastName, string Password) : ICommand<RegisterResponse>;
}
