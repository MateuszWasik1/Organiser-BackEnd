using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Helpers;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers
{
    public class ChangeBugStatusCommandHandler : ICommandHandler<ChangeBugStatusCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public ChangeBugStatusCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(ChangeBugStatusCommand command)
        {
            var bug = context.Bugs.FirstOrDefault(x => x.BGID == command.Model.BGID);

            if (bug == null)
                throw new Exception("Nie udało się zaaktualizować statusu błędu!");
            bug.BStatus = command.Model.Status;

            var currentUser = context.User.FirstOrDefault(x => x.UID == user.UID);

            var bugNote = new Cores.Entities.BugsNotes()
            {
                BNGID = Guid.NewGuid(),
                BNBGID = bug.BGID,
                BNUID = user.UID,
                BNDate = DateTime.Now,
                BNText = $"Status został zmieniony na: \"{ChangeBugStatusToText.BugStatusText(command.Model.Status)}\" przez użytkownika: {currentUser?.UFirstName} {currentUser?.ULastName}",
                BNIsNewVerifier = false,
                BNIsStatusChange = true,
                BNChangedStatus = command.Model.Status,
            };

            context.CreateOrUpdate(bugNote);
            context.CreateOrUpdate(bug);
            context.SaveChanges();
        }
    }
}
