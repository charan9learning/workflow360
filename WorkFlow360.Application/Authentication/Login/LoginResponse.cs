namespace WorkFlow360.Application.Authentication.Login
{
    public sealed record LoginResponse(Guid UserId, string Email, string FirstName, string LastName, string Role, string AccessToken);
}
