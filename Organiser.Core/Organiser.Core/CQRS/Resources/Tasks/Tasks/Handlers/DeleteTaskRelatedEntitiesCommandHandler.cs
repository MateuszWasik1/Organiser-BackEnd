using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class DeleteTaskRelatedEntitiesCommandHandler : ICommandHandler<DeleteTaskRelatedEntitiesCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteTaskRelatedEntitiesCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteTaskRelatedEntitiesCommand command)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.Model.TGID);

            if (task == null)
                throw new TaskNotFoundException("Nie znaleziono zadania");

            if (command.Model.DeleteTaskNotes)
            {
                var taskNotes = context.TasksNotes.Where(x => x.TNTGID == task.TGID).ToList();

                foreach (var taskNote in taskNotes)
                    context.DeleteTaskNotes(taskNote);
            }

            if(command.Model.DeleteTaskSubTasks)
            {
                var taskSubTasks= context.TasksSubTasks.Where(x => x.TSTTGID == task.TGID);

                foreach (var taskSubTask in taskSubTasks)
                    context.DeleteTaskSubTask(taskSubTask);
            }

            context.DeleteTask(task);
            context.SaveChanges();
        }
    }
}
