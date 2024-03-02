using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class SaveTaskCommandHandler : ICommandHandler<SaveTaskCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public SaveTaskCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(SaveTaskCommand command)
        {
            if (command.Model.TID == 0)
            {
                var task = new Cores.Entities.Tasks()
                {
                    TGID = command.Model.TGID,
                    TUID = user.UID,
                    TCGID = command.Model.TCGID,
                    TName = command.Model.TName,
                    TLocalization = command.Model.TLocalization,
                    TTime = command.Model.TTime,
                    TBudget = command.Model.TBudget,
                    TStatus = command.Model.TStatus,
                };

                context.CreateOrUpdate(task);
            }
            else
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
            }

            context.SaveChanges();
        }
    }
}
