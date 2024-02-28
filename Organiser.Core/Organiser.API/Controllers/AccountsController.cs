using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels.AccountsViewModel;
using Organiser.Cores.Services.EmailSender;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IPasswordHasher<User> hasher;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly IEmailSender emailSender;
        public AccountsController(IDataBaseContext context, 
            IPasswordHasher<User> hasher,
            AuthenticationSettings authenticationSettings,
            IEmailSender emailSender)
        {
            this.context = context;
            this.hasher = hasher;
            this.authenticationSettings = authenticationSettings;
            this.emailSender = emailSender;
        }

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

            var userNameExist = context.AllUsers.Any(x => x.UUserName == model.UUserName);

            if (userNameExist)
                throw new Exception("Podana nazwa użytkownika występuje w systemie");

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

            var hashedPassword = hasher.HashPassword(newUser, newUser.UPassword);

            newUser.UPassword = hashedPassword;

            context.CreateOrUpdate(newUser);
            context.SaveChanges();

            //emailSender.SendEmail(newUser.UEmail, "Witaj!", "Witaaaaj!");
        }

        [HttpGet]
        [Route("Login")]
        public string Login(string username, string password)
        {
            var model = new LoginViewModel()
            {
                UUserName = username,
                UPassword = password,
            };

            var token = GenerateJWTToken(model);

            return JsonSerializer.Serialize(token);
        }

        public string GenerateJWTToken(LoginViewModel model)
        {
            var user = context.AllUsers.FirstOrDefault(u => u.UUserName == model.UUserName);

            if (user == null)
                throw new Exception("Podany login lub hasło jest błędne!");

            var result = hasher.VerifyHashedPassword(user, user.UPassword, model.UPassword);

            if(result == PasswordVerificationResult.Failed)
                throw new Exception("Podany login lub hasło jest błędne!");

            var userRole = context.Roles.FirstOrDefault(x => x.RID == user.URID)?.RName ?? "User";

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UGID.ToString()),
                new Claim(ClaimTypes.Name, $"{user.UFirstName} {user.ULastName}"),
                new Claim(ClaimTypes.Role, $"{userRole}"),
                new Claim("UID", user.UID.ToString()),
                new Claim("UGID", user.UGID.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JWTKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JWTExpiredDays);

            var token = new JwtSecurityToken(authenticationSettings.JWTIssuer, authenticationSettings.JWTIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}