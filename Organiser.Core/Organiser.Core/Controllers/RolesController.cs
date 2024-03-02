using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public RolesController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetUserRoles")]
        public RolesViewModel GetUserRoles()
            => dispatcher.DispatchQuery<GetUserRolesQuery, RolesViewModel>(new GetUserRolesQuery());

        [HttpGet]
        [Route("GetIsUserAdmin")]
        public bool GetIsUserAdmin() 
            => dispatcher.DispatchQuery<GetIsUserAdminQuery, bool>(new GetIsUserAdminQuery());

        [HttpGet]
        [Route("GetIsUserSupport")]
        public bool GetIsUserSupport() 
            => dispatcher.DispatchQuery<GetIsUserSupportQuery, bool>(new GetIsUserSupportQuery());
    }
}
