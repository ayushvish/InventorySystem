namespace Inventory.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string userName, string email, string role, IEnumerable<string> permissions);
}
