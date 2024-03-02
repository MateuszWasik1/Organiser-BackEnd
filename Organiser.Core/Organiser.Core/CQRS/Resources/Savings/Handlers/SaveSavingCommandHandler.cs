using AutoMapper;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class SaveSavingCommandHandler : ICommandHandler<SaveSavingCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public SaveSavingCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(SaveSavingCommand command)
        {
            if (command.Model.SID == 0)
            {
                var saving = new Cores.Entities.Savings()
                {
                    SGID = command.Model.SGID,
                    SUID = user.UID,
                    SAmount = command.Model.SAmount,
                    STime = command.Model.STime,
                    SOnWhat = command.Model.SOnWhat,
                    SWhere = command.Model.SWhere,
                };

                context.CreateOrUpdate(saving);
            }
            else
            {
                var saving = context.Savings.FirstOrDefault(x => x.SGID == command.Model.SGID);

                if (saving == null)
                    throw new Exception("Nie znaleziono oszczędności");

                saving.SAmount = command.Model.SAmount;
                saving.STime = command.Model.STime;
                saving.SOnWhat = command.Model.SOnWhat;
                saving.SWhere = command.Model.SWhere;
            }

            context.SaveChanges();
        }
    }
}
