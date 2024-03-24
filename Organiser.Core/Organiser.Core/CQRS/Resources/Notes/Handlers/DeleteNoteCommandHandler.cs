using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.Exceptions.Notes;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class DeleteNoteCommandHandler : ICommandHandler<DeleteNoteCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteNoteCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteNoteCommand command)
        {
            var note = context.Notes.FirstOrDefault(x => x.NGID == command.NGID);

            if (note == null)
                throw new NoteNotFoundException("Nie udało się znaleźć podanej notatki. Notatka nie zostanie usunięta");

            context.DeleteNote(note);
            context.SaveChanges();
        }
    }
}
