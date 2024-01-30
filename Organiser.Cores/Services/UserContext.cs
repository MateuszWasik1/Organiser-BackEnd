﻿using System.Security.Claims;

namespace Organiser.Cores.Services
{
    public interface IUserContext
    {
        ClaimsPrincipal? User { get; }
        int? UID { get; }
        string? GUID { get; }
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor) 
        { 
            this.httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
        public int? UID => User == null ? null : int.Parse(User?.FindFirst(x => x.Type == "UID")?.Value);
        public string? GUID => User == null ? null : User?.FindFirst(x => x.Type == "GUID")?.Value;
    }
}
