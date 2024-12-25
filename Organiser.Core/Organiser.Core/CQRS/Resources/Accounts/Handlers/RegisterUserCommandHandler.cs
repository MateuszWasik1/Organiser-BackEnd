using Microsoft.AspNetCore.Identity;
using Organiser.Core.CQRS.Resources.Accounts.Commands;
using Organiser.Core.Exceptions.Accounts;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services.EmailSender;
using Organiser.CQRS.Abstraction.Commands;
using System.Text.RegularExpressions;

namespace Organiser.Core.CQRS.Resources.Accounts.Handlers
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IPasswordHasher<Cores.Entities.User> hasher;
        private readonly IEmailSender emailSender;

        public RegisterUserCommandHandler(IDataBaseContext context, IPasswordHasher<Cores.Entities.User> hasher, IEmailSender emailSender) 
        {
            this.context = context;
            this.hasher = hasher;
            this.emailSender = emailSender;
        }

        public void Handle(RegisterUserCommand command)
        {
            if (string.IsNullOrEmpty(command.Model.UUserName))
                throw new RegisterUserNameIsEmptyException("Nazwa użytkownika nie może być pusta");

            if (command.Model.UUserName.Length > 100)
                throw new RegisterUserNameIsOver100Exception("Nazwa użytkownika nie może zawierać więcej niż 100 znaków!");

            if (string.IsNullOrEmpty(command.Model.UEmail))
                throw new RegisterEmailIsEmptyException("Email nie może być pusty");

            if (command.Model.UEmail.Length > 100)
                throw new RegisterEmailIsOver100Exception("Email użytkownika nie może zawierać więcej niż 100 znaków!");

            if (string.IsNullOrEmpty(command.Model.UPassword))
                throw new RegisterPasswordIsEmptyException("Hasło nie może być puste");

            if (!Regex.IsMatch(command.Model.UPassword, "[0-9]"))
                throw new RegisterPasswordNoNumbersException("Hasło nie zawiera cyfr");

            if (!Regex.IsMatch(command.Model.UPassword, "[A-Z]"))
                throw new RegisterPasswordNoUpperCaseException("Hasło nie zawiera wielkich liter");

            if (!Regex.IsMatch(command.Model.UPassword, "[a-z]"))
                throw new RegisterPasswordNoLowerCaseException("Hasło nie zawiera małych liter");

            if (!Regex.IsMatch(command.Model.UPassword, "[$@^!%*?&]"))
                throw new RegisterPasswordNoSpecialSignsException("Hasło nie zawiera znaków specjalnych");

            if (command.Model.UPassword.Length < 8)
                throw new RegisterPasswordNo8charactersException("Hasło jest krótsze niż 8 znaków!");

            var userNameExist = context.AllUsers.Any(x => x.UUserName == command.Model.UUserName);

            if (userNameExist)
                throw new RegisterUserNameIsFoundException("Podana nazwa użytkownika występuje w systemie");

            var roleID = context.Roles.FirstOrDefault(x => x.RName == "user")?.RID ?? (int) RoleEnum.User;

            var newUser = new Cores.Entities.User()
            {
                UGID = Guid.NewGuid(),
                URID = roleID,
                UFirstName = "",
                ULastName = "",
                UUserName = command.Model.UUserName,
                UEmail = command.Model.UEmail,
                UPhone = "",
                UPassword = command.Model.UPassword,
            };

            var hashedPassword = hasher.HashPassword(newUser, newUser.UPassword);

            newUser.UPassword = hashedPassword;

            context.CreateOrUpdate(newUser);
            context.SaveChanges();

            //emailSender.SendEmail(newUser.UEmail, "Witaj!", "Witaaaaj!");
        }
    }
}
