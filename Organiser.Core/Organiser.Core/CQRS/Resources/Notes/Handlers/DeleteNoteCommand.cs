using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class DeleteNoteCommand : ICommandHandler<AddNoteCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteNoteCommand(IDataBaseContext context) => this.context = context;

        public void Handle(AddNoteCommand command)
        {
            var note = context.Notes.FirstOrDefault(x => x.NGID == command.Model.NGID);

            if (note == null)
                throw new Exception("Nie udało się znaleźć podanej notatki. Notatka nie zostanie usunięta");

            context.DeleteNote(note);
            context.SaveChanges();
        }
    }
}
