using Microsoft.AspNetCore.Identity;

namespace Organiser.Cores.Entities
{
    public class Users : IdentityUser
    {
        public string? UsrEmail { get; set; }
    }
}
