using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers
{
    public class SaveBugsCommandHandler : ICommandHandler<SaveBugCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public SaveBugsCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(SaveBugCommand command)
        {
            var bug = new Cores.Entities.Bugs()
            {
                BGID = command.Model.BGID,
                BUID = user.UID,
                BDate = DateTime.Now,
                BTitle = command.Model.BTitle,
                BText = command.Model.BText,
                BStatus = command.Model.BStatus,
            };

            context.CreateOrUpdate(bug);
            context.SaveChanges();
        }
    }
}
