namespace WorkFlow360.Application.Authentication.Register
{
    public sealed record RegisterResponse(Guid Id, string Email, string FirstName, string LastName);
}
