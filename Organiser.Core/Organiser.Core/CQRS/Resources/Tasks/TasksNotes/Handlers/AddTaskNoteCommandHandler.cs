using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers
{
    public class AddTaskNoteCommandHandler : ICommandHandler<AddTaskNoteCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddTaskNoteCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddTaskNoteCommand command)
        {
            if (command.Model.TNNote.Length == 0)
                throw new TaskNotesTextRequiredException("Nazwa zadania jest wymagana!");

            if (command.Model.TNNote.Length > 2000)
                throw new TaskNotesTextMax2000Exception("Nazwa zadania nie może być dłuższa niż 2000 znaków!");

            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.Model.TNTGID);

            if (task == null)
                throw new TaskNotFoundException("Nie znaleziono zadania! Nie udało się dodać notatki do zadania");

            var taskNote = new Cores.Entities.TasksNotes()
            {
                TNGID = command.Model.TNGID,
                TNTGID = command.Model.TNTGID,
                TNUID = user.UID,
                TNNote = command.Model.TNNote,
                TNDate = DateTime.Now,
            };

            context.CreateOrUpdate(taskNote);

            context.SaveChanges();
        }
    }
}
