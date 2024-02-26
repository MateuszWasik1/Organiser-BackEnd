using System.Security.Claims;

namespace Organiser.Cores.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
        public int UID => User == null ? 1 : int.Parse(User?.FindFirst(x => x.Type == "UID")?.Value);
        public string? UGID => User == null ? null : User?.FindFirst(x => x.Type == "UGID")?.Value;
    }
}
