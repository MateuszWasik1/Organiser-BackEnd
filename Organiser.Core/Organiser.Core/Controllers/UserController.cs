using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Cores.Models.ViewModels.UserViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public UserController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public List<UsersAdminViewModel> GetAllUsers(string text = "")
            => dispatcher.DispatchQuery<GetAllUsersQuery, List<UsersAdminViewModel>>(new GetAllUsersQuery());

        [HttpGet]
        [Route("GetUserByAdmin/{ugid}")]
        [Authorize(Roles = "Admin")]
        public UserAdminViewModel GetUserByAdmin(Guid ugid)
            => dispatcher.DispatchQuery<GetUserByAdminQuery, UserAdminViewModel>(new GetUserByAdminQuery() { UGID = ugid });

        [HttpGet]
        [Route("GetUser")]
        [Authorize]
        public UserViewModel GetUser()
            => dispatcher.DispatchQuery<GetUserQuery, UserViewModel>(new GetUserQuery());

        [HttpPost]
        [Route("SaveUser")]
        [Authorize]
        public void SaveUser(UserViewModel model)
            => dispatcher.DispatchCommand(new SaveUserCommand() { Model = model });

        [HttpPost]
        [Route("SaveUserByAdmin")]
        [Authorize(Roles = "Admin")]
        public void SaveUserByAdmin(UserAdminViewModel model)
            => dispatcher.DispatchCommand(new SaveUserByAdminCommand() { Model = model });

        [HttpDelete]
        [Route("DeleteUser/{ugid}")]
        [Authorize(Roles = "Admin")]
        public void DeleteUser(Guid ugid)
            => dispatcher.DispatchCommand(new DeleteUserCommand() { UGID = ugid });
    }
}