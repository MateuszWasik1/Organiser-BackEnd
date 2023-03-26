using Microsoft.AspNetCore.Identity;

namespace Organiser.Core.Entities
{
    public class Users : IdentityUser
    {
        public string? UsrEmail { get; set; }
    }
}
