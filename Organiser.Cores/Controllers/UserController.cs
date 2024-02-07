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
        [Authorize("Admin")]
        public List<UserAdminViewModel> GetAllUsers(string text = "")
        {
            var userData = context.AllUsers.ToList();

            var userAdmViewModel = new List<UserAdminViewModel>();

            userData.ForEach(x => {
                var model = mapper.Map<User, UserAdminViewModel>(x);
                userAdmViewModel.Add(model);
            });

            return userAdmViewModel;
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
    }
}