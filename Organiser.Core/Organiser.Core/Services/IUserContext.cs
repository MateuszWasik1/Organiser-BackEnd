using System.Security.Claims;

namespace Organiser.Cores.Services
{
    public interface IUserContext
    {
        ClaimsPrincipal? User { get; }
        int UID { get; }
        string? UGID { get; }
    }
}
