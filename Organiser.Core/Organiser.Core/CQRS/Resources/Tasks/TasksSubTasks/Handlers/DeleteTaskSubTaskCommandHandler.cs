using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class DeleteTaskSubTaskCommandHandler : ICommandHandler<DeleteTaskSubTaskCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteTaskSubTaskCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteTaskSubTaskCommand command)
        {
            var subtask = context.TasksSubTasks.FirstOrDefault(x => x.TSTGID == command.TSTGID);

            if (subtask == null)
                throw new TasksSubTasksNotFoundExceptions("Nie znaleziono podzadania!");

            context.DeleteTask(subtask);
            context.SaveChanges();
        }
    }
}
