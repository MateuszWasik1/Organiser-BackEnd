using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
    {
        private readonly IDataBaseContext context;
        public UpdateTaskCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(UpdateTaskCommand command)
        {
            if (command.Model.TName.Length == 0)
                throw new TaskNameRequiredException("Nazwa zadania jest wymagana!");

            if (command.Model.TName.Length > 300)
                throw new TaskNameMax300Exception("Nazwa zadania nie może być dłuższa niż 300 znaków!");

            if (command.Model.TLocalization.Length > 300)
                throw new TaskLocalizationMax300Exception("Lokalizacja zadania nie może być dłuższa niż 300 znaków!");

            if (command.Model.TBudget < 0)
                throw new TaskBudgetMin0Exception("Budżet nie może być mniejszy niż 0!");

            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.Model.TGID);

            if (task == null)
                throw new TaskNotFoundException("Nie znaleziono zadania");

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
