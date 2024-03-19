using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
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
            context.SaveChanges();
        }
    }
}
