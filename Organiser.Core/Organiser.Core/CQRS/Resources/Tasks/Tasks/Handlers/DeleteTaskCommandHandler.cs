using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
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
                throw new Exception("Nie znaleziono zadania");

            context.DeleteTask(task);
            context.SaveChanges();
        }
    }
}
