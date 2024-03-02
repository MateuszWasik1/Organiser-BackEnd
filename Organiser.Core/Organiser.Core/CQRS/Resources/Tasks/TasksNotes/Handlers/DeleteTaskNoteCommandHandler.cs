using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers
{
    public class DeleteTaskNoteCommandHandler : ICommandHandler<DeleteTaskNoteCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteTaskNoteCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteTaskNoteCommand command)
        {
            var taskNote = context.TasksNotes.FirstOrDefault(x => x.TNGID == command.TNGID);

            if (taskNote == null)
                throw new Exception("Nie znaleziono notatki do zadania! Notatka nie została usunięta.");

            context.DeleteTaskNotes(taskNote);
            context.SaveChanges();
        }
    }
}
