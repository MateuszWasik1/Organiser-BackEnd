using Microsoft.AspNetCore.Identity;

namespace Organiser.Cores.Entities
{
    public class Users : IdentityUser
    {
        public string? UFullName { get; set; }
        public string? UUserName { get; set; }
        public string? UEmail { get; set; }
        public string? UPhone { get; set; }
        public string? UPassword { get; set; }
    }
}