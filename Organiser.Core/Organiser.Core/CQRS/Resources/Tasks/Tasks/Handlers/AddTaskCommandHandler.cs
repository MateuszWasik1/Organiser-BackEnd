using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class AddTaskCommandHandler : ICommandHandler<AddTaskCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddTaskCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddTaskCommand command)
        {
            if (command.Model.TName.Length == 0)
                throw new TaskNameRequiredException("Nazwa zadania jest wymagana!");

            if (command.Model.TName.Length > 300)
                throw new TaskNameMax300Exception("Nazwa zadania nie może być dłuższa niż 300 znaków!");

            if (command.Model.TLocalization.Length > 300)
                throw new TaskLocalizationMax300Exception("Lokalizacja zadania nie może być dłuższa niż 300 znaków!");

            if (command.Model.TBudget < 0)
                throw new TaskBudgetMin0Exception("Budżet nie może być mniejszy niż 0!");

            var task = new Cores.Entities.Tasks()
            {
                TGID = Guid.NewGuid(),
                TUID = user.UID,
                TCGID = command.Model.TCGID,
                TName = command.Model.TName,
                TLocalization = command.Model.TLocalization,
                TTime = command.Model.TTime,
                TBudget = command.Model.TBudget,
                TStatus = command.Model.TStatus,
            };

            context.CreateOrUpdate(task);
            context.SaveChanges();
        }
    }
}
