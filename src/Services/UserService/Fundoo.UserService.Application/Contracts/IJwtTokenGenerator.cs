using Fundoo.UserService.Domain.Entities;

namespace Fundoo.UserService.Application.Contracts;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}