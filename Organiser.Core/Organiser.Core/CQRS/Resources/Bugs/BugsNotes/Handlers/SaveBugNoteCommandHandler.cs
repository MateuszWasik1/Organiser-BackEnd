using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands;
using Organiser.Core.Exceptions.Accounts;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Handlers
{
    public class SaveBugNoteCommandHandler : ICommandHandler<SaveBugNoteCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public SaveBugNoteCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(SaveBugNoteCommand command) 
        {
            if (command.Model.BNText.Length == 0)
                throw new BugsNotesTextRequiredException("Tekst notatki do błędu musi zawierać znaki!");

            var bugNote = new Cores.Entities.BugsNotes()
            {
                BNGID = Guid.NewGuid(),
                BNBGID = command.Model.BNBGID,
                BNUID = user.UID,
                BNDate = DateTime.Now,
                BNText = command.Model.BNText,
                BNIsNewVerifier = false,
                BNIsStatusChange = false,
            };

            var currentUser = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (currentUser == null)
                throw new UserNotFoundExceptions("Nie znaleziono użytkownika");

            var bug = context.AllBugs.FirstOrDefault(x => x.BGID == bugNote.BNBGID);

            if (bug == null)
                throw new BugNotFoundExceptions("Nie znaleziono wskazanego błędu!");

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
                    BNBGID = command.Model.BNBGID,
                    BNUID = user.UID,
                    BNDate = DateTime.Now,
                    BNText = $"Nowym weryfikującym jest: {currentUser?.UFirstName} {currentUser?.ULastName} {currentUser?.UGID}",
                    BNIsNewVerifier = true,
                    BNIsStatusChange = false,
                };
                context.CreateOrUpdate(bugNoteIsVerifier);
            }

            context.CreateOrUpdate(bugNote);
            context.SaveChanges();
        }
    }
}
