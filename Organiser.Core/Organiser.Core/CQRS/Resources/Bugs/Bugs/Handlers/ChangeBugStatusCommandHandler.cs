using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.Exceptions;
using Organiser.Core.Exceptions.Bugs;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
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
            var bug = context.AllBugs.FirstOrDefault(x => x.BGID == command.Model.BGID);

            if (bug == null)
                throw new BugNotFoundExceptions("Nie udało się zaaktualizować statusu błędu!");

            bug.BStatus = command.Model.Status;

            var currentUser = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (currentUser == null)
                throw new UserNotFoundExceptions("Nie udało się odnaleźć użytkownika! Aktualizacja błędu się nie powiodła.");

            var isUserVerifier = bug?.BAUIDS?.Contains(currentUser.UGID.ToString()) ?? false;
            var isUserSupportOrAdmin = (currentUser?.URID == (int)RoleEnum.Admin || currentUser?.URID == (int)RoleEnum.Support);

            if (!isUserVerifier && isUserSupportOrAdmin)
            {
                if (string.IsNullOrEmpty(bug?.BAUIDS))
                    bug.BAUIDS = currentUser?.UGID.ToString();
                else
                    bug.BAUIDS = string.Join(",", bug.BAUIDS, currentUser?.UGID);

                context.CreateOrUpdate(bug);

                var bugNoteIsVerifier = new Cores.Entities.BugsNotes()
                {
                    BNGID = Guid.NewGuid(),
                    BNBGID = bug.BGID,
                    BNUID = user.UID,
                    BNDate = DateTime.Now,
                    BNText = $"Nowym weryfikującym jest: {currentUser?.UFirstName} {currentUser?.ULastName} {currentUser?.UGID}",
                    BNIsNewVerifier = true,
                    BNIsStatusChange = false,
                };
                context.CreateOrUpdate(bugNoteIsVerifier);
            }

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