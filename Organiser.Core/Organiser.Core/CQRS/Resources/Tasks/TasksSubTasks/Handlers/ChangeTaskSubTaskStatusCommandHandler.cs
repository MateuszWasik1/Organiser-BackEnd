using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers
{
    public class ChangeTaskSubTaskStatusCommandHandler : ICommandHandler<ChangeTaskSubTaskStatusCommand>
    {
        private readonly IDataBaseContext context;
        public ChangeTaskSubTaskStatusCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(ChangeTaskSubTaskStatusCommand command)
        {
            var subtask = context.TasksSubTasks.FirstOrDefault(x => x.TSTGID == command.Model.TSTGID);

            if (subtask == null)
                throw new TasksSubTasksNotFoundExceptions("Nie udało się zaaktualizować statusu podzadania!");

            subtask.TSTStatus = command.Model.Status;

            context.CreateOrUpdate(subtask);
            context.SaveChanges();
        }
    }
}