using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public RolesController(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public RolesViewModel GetUserRoles()
        {
            var userRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            var model = new RolesViewModel()
            {
                IsAdmin = userRole == (int) RoleEnum.Admin,
                IsSupport = userRole == (int) RoleEnum.Admin || userRole == (int) RoleEnum.Support,
                IsUser = userRole == (int) RoleEnum.Admin || userRole == (int) RoleEnum.Support || userRole == (int)RoleEnum.User,
            };

            return model;
        }

        [HttpGet]
        [Route("GetIsUserAdmin")]
        public bool GetIsUserAdmin() => context.User.FirstOrDefault(x => x.UID == user.UID)?.URID == (int) RoleEnum.Admin;

        [HttpGet]
        [Route("GetIsUserSupport")]
        public bool GetIsUserSupport() => context.User.FirstOrDefault(x => x.UID == user.UID)?.URID == (int) RoleEnum.Support;
    }
}
