using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
    {
        private readonly IDataBaseContext context;
        public UpdateTaskCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(UpdateTaskCommand command)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.Model.TGID);

            if (task == null)
                throw new Exception("Nie znaleziono zadania");

            task.TCGID = command.Model.TCGID;
            task.TName = command.Model.TName;
            task.TLocalization = command.Model.TLocalization;
            task.TTime = command.Model.TTime;
            task.TBudget = command.Model.TBudget;
            task.TStatus = command.Model.TStatus;

            context.CreateOrUpdate(task);
            context.SaveChanges();
        }
    }
}
