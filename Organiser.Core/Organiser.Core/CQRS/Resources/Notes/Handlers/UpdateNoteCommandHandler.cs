using Organiser.Core.CQRS.Resources.Notes.Commands;
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
            var note = context.Notes.FirstOrDefault(x => x.NGID == command.Model.NGID);

            if (note == null)
                throw new Exception("Nie udało się znaleźć podanej notatki. Notatka nie zostanie zmodyfikowana");

            note.NTitle = command.Model.NTitle;
            note.NTxt = command.Model.NTxt;
            note.NModificationDate = DateTime.Now;

            context.CreateOrUpdate(note);
            context.SaveChanges();
        }
    }
}
