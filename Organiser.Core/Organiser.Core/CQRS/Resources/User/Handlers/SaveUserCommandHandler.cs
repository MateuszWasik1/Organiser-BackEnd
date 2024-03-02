using Organiser.Core.CQRS.Resources.User.Commands;
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
            var userData = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");

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
