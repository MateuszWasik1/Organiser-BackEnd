using Microsoft.AspNetCore.Identity;
using Organiser.Core.CQRS.Resources.Accounts.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Services.EmailSender;
using Organiser.CQRS.Abstraction.Commands;

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
                throw new Exception("Nazwa użytkownika nie może być pusta");

            if (string.IsNullOrEmpty(command.Model.UEmail))
                throw new Exception("Email nie może być pusty");

            if (string.IsNullOrEmpty(command.Model.UPassword))
                throw new Exception("Hasło nie może być puste");

            var userNameExist = context.AllUsers.Any(x => x.UUserName == command.Model.UUserName);

            if (userNameExist)
                throw new Exception("Podana nazwa użytkownika występuje w systemie");

            var roleID = context.Roles.FirstOrDefault(x => x.RName == "user")?.RID ?? 1;

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
