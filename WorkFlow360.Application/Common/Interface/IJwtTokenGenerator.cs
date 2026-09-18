using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Common.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
