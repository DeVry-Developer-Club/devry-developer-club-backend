using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Models;

namespace DevryDeveloperClub.Infrastructure.Services
{
    public interface IJwtService
    {
        Task<JwtSecurityToken> GenerateUserToken(ClubMember user);
    }
}