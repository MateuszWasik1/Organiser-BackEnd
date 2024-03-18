using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class UpdateSavingCommandHandler : ICommandHandler<UpdateSavingCommand>
    {
        private readonly IDataBaseContext context;
        public UpdateSavingCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(UpdateSavingCommand command)
        {
            var saving = context.Savings.FirstOrDefault(x => x.SGID == command.Model.SGID);
            
            if (saving == null)
                throw new Exception("Nie znaleziono oszczędności");
            
            saving.SAmount = command.Model.SAmount;
            saving.STime = command.Model.STime;
            saving.SOnWhat = command.Model.SOnWhat;
            saving.SWhere = command.Model.SWhere;

            context.CreateOrUpdate(saving);  
            context.SaveChanges();
        }
    }
}
