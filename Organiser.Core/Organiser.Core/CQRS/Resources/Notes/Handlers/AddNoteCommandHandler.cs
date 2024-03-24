using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.Exceptions.Notes;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class AddNoteCommandHandler : ICommandHandler<AddNoteCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddNoteCommandHandler(IDataBaseContext context, IUserContext user) 
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddNoteCommand command)
        {
            if (command.Model.NTitle.Length == 0)
                throw new NoteTitleRequiredException("Tytuł notatki nie może być pusty!");

            if (command.Model.NTitle.Length > 200)
                throw new NoteTitleMax200Exception("Tytuł notatki nie może być przekraczać 200 znaków!");

            if (command.Model.NTxt.Length == 0)
                throw new NoteTextRequiredException("Tekst notatki nie może być pusty!");

            if (command.Model.NTxt.Length > 4000)
                throw new NoteTitleMax4000Exception("Tekst notatki nie może być przekraczać 4000 znaków!");

            var note = new Cores.Entities.Notes()
            {
                NUID = user.UID,
                NGID = Guid.NewGuid(),
                NDate = DateTime.Now,
                NModificationDate = DateTime.Now,
                NTitle = command.Model.NTitle,
                NTxt = command.Model.NTxt,
            };

            context.CreateOrUpdate(note);
            context.SaveChanges();
        }
    }
}
