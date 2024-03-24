using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.Exceptions;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class SaveUserCommandHandler : ICommandHandler<SaveUserCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public SaveUserCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }
        public void Handle(SaveUserCommand command)
        {
            if (command.Model.UUserName.Length == 0)
                throw new UserNameRequiredException("Nazwa użytkownika nie może być pusta!");

            if (command.Model.UUserName.Length > 100)
                throw new UserNameMax100Exception("Nazwa użytkownika nie może mieć więcej niż 100 znaków!");

            if (command.Model.UFirstName.Length > 50)
                throw new UserFirstNameMax50Exception("Imię użytkownika nie może mieć więcej niż 50 znaków!");

            if (command.Model.ULastName.Length > 50)
                throw new UserLastNameMax50Exception("Nazwisko użytkownika nie może mieć więcej niż 50 znaków!");

            if (command.Model.UEmail.Length == 0)
                throw new UserEmailRequiredException("Email użytkownika nie może być pusty!");

            if (command.Model.UEmail.Length > 100)
                throw new UserEmailMax100Exception("Email użytkownika nie może mieć więcej niż 100 znaków!");

            if (command.Model.UPhone.Length > 100)
                throw new UserPhoneMax100Exception("Telefon użytkownika nie może mieć więcej niż 100 znaków!");

            var userData = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (userData == null)
                throw new UserNotFoundExceptions("Nie znaleziono użytkownika!");

            userData.UFirstName = command.Model.UFirstName;
            userData.ULastName = command.Model.ULastName;
            userData.UUserName = command.Model.UUserName;
            userData.UEmail = command.Model.UEmail;
            userData.UPhone = command.Model.UPhone;

            context.CreateOrUpdate(userData);
            context.SaveChanges();
        }
    }
}
