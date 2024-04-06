using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteTaskCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteTaskCommand command)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.TGID);

            if (task == null)
                throw new TaskNotFoundException("Nie znaleziono zadania");

            var taskNotesCount = context.TasksNotes.Count(x => x.TNTGID == task.TGID);

            if (taskNotesCount > 0)
                throw new TaskConatinsTaskNotesException("Do zadania przypisane są notatki! Usuń je najpierw!");

            var taskSubTasksCount = context.TasksSubTasks.Count(x => x.TSTTGID == task.TGID);

            if (taskSubTasksCount > 0)
                throw new TaskConatinsSubTasksException("Do zadania przypisane są podzadania! Usuń je najpierw!");

            context.DeleteTask(task);
            context.SaveChanges();
        }
    }
}
