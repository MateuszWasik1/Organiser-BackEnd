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
        [Route("GetIsPremiumUser")]
        public bool GetIsPremiumUser()
            => dispatcher.DispatchQuery<GetIsPremiumUserQuery, bool>(new GetIsPremiumUserQuery());

        [HttpGet]
        [Route("GetIsAnyUser")]
        public bool GetIsAnyUser()
            => dispatcher.DispatchQuery<GetIsAnyUserQuery, bool>(new GetIsAnyUserQuery());

        [HttpGet]
        [Route("GetIsUserSupport")]
        public bool GetIsUserSupport() 
            => dispatcher.DispatchQuery<GetIsUserSupportQuery, bool>(new GetIsUserSupportQuery());

        [HttpGet]
        [Route("GetIsUserAdmin")]
        public bool GetIsUserAdmin()
            => dispatcher.DispatchQuery<GetIsUserAdminQuery, bool>(new GetIsUserAdminQuery());
    }
}
