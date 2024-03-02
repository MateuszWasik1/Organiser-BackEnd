using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class SaveUserByAdminCommandHandler : ICommandHandler<SaveUserByAdminCommand>
    {
        private readonly IDataBaseContext context;
        public SaveUserByAdminCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(SaveUserByAdminCommand command)
        {
            var userData = context.AllUsers.FirstOrDefault(x => x.UGID == command.Model.UGID);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");

            userData.URID = command.Model.URID;
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
