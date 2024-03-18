using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class AddSavingCommandHandler : ICommandHandler<AddSavingCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddSavingCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddSavingCommand command)
        {
            var saving = new Cores.Entities.Savings()
            {
                SGID = Guid.NewGuid(),
                SUID = user.UID,
                SAmount = command.Model.SAmount,
                STime = command.Model.STime,
                SOnWhat = command.Model.SOnWhat,
                SWhere = command.Model.SWhere,
            };

            context.CreateOrUpdate(saving);
            context.SaveChanges();
        }
    }
}
