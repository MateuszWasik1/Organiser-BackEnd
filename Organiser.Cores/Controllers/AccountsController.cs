using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IDataBaseContext context;
        public AccountsController(IDataBaseContext context) => this.context = context;

        [HttpPost]
        [Route("Register")]
        public void Register(RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.UUserName))
                throw new Exception("Nazwa użytkownika nie może być pusta");

            if (string.IsNullOrEmpty(model.UEmail))
                throw new Exception("Email nie może być pusty");

            if (string.IsNullOrEmpty(model.UPassword))
                throw new Exception("Hasło nie może być puste");

            var roleID = context.Roles.FirstOrDefault(x => x.RName == "user")?.RID ?? 1;

            var newUser = new User()
            {
                UGID = Guid.NewGuid(),
                URID = roleID,
                UFirstName = "",
                ULastName = "",
                UUserName = model.UUserName,
                UEmail = model.UEmail,
                UPhone = "",
                UPassword = model.UPassword,
            };

            context.CreateOrUpdate(newUser);
            context.SaveChanges();
        }
    }
}