using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.Exceptions.Notes;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class UpdateNoteCommandHandler : ICommandHandler<UpdateNoteCommand>
    {
        private readonly IDataBaseContext context;
        public UpdateNoteCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(UpdateNoteCommand command)
        {
            if (command.Model.NTitle.Length == 0)
                throw new NoteTitleRequiredException("Tytuł notatki nie może być pusty!");

            if (command.Model.NTitle.Length > 200)
                throw new NoteTitleMax200Exception("Tytuł notatki nie może być przekraczać 200 znaków!");

            if (command.Model.NTxt.Length == 0)
                throw new NoteTextRequiredException("Tekst notatki nie może być pusty!");

            if (command.Model.NTxt.Length > 4000)
                throw new NoteTitleMax4000Exception("Tekst notatki nie może być przekraczać 4000 znaków!");

            var note = context.Notes.FirstOrDefault(x => x.NGID == command.Model.NGID);

            if (note == null)
                throw new NoteNotFoundException("Nie udało się znaleźć podanej notatki. Notatka nie zostanie zmodyfikowana");

            note.NTitle = command.Model.NTitle;
            note.NTxt = command.Model.NTxt;
            note.NModificationDate = DateTime.Now;

            context.CreateOrUpdate(note);
            context.SaveChanges();
        }
    }
}
