using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.Cores.Services;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public UserController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public List<UsersAdminViewModel> GetAllUsers(string text = "")
        {
            var usersData = context.AllUsers.ToList();

            var usersAdmViewModel = new List<UsersAdminViewModel>();

            usersData.ForEach(x => {
                var model = mapper.Map<User, UsersAdminViewModel>(x);
                usersAdmViewModel.Add(model);
            });

            return usersAdmViewModel;
        }

        [HttpGet]
        [Route("GetUserByAdmin/{ugid}")]
        [Authorize(Roles = "Admin")]
        public UserAdminViewModel GetUserByAdmin(Guid ugid)
        {
            var userData = context.AllUsers.FirstOrDefault(x => x.UGID == ugid);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");
            
            var model = mapper.Map<User, UserAdminViewModel>(userData);

            model.UCategoriesCount = context.AllCategories.Where(x => x.CUID == userData.UID).Count();
            model.UTasksCount = context.AllTasks.Where(x => x.TUID == userData.UID).Count();
            model.UTaskNotesCount = context.AllTasksNotes.Where(x => x.TNUID == userData.UID).Count();
            model.USavingsCount = context.AllSavings.Where(x => x.SUID == userData.UID).Count();

            return model;
        }

        [HttpGet]
        [Route("GetUser")]
        [Authorize]
        public UserViewModel GetUser()
        {
            var userData = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");

            var model = mapper.Map<User, UserViewModel>(userData);
            
            return model;
        }

        [HttpPost]
        [Route("SaveUser")]
        [Authorize]
        public void SaveUser(UserViewModel model)
        {
           var userData = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");

            userData.UFirstName = model.UFirstName;
            userData.ULastName = model.ULastName;
            userData.UUserName = model.UUserName;
            userData.UEmail = model.UEmail;
            userData.UPhone = model.UPhone;

            context.CreateOrUpdate(userData);
            context.SaveChanges();
        }

        [HttpPost]
        [Route("SaveUserByAdmin")]
        [Authorize(Roles = "Admin")]
        public void SaveUserByAdmin(UserAdminViewModel model)
        {
            var userData = context.User.FirstOrDefault(x => x.UGID == model.UGID);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");

            userData.URID = model.URID;
            userData.UFirstName = model.UFirstName;
            userData.ULastName = model.ULastName;
            userData.UUserName = model.UUserName;
            userData.UEmail = model.UEmail;
            userData.UPhone = model.UPhone;

            context.CreateOrUpdate(userData);
            context.SaveChanges();
        }
    }
}