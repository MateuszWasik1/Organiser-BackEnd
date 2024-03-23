using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.Exceptions.Savings;
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
            if (command.Model.SAmount < 0)
                throw new SavingAmountLessThan0Exception("Kwota oszczędności nie może być niższa niż 0!");

            var saving = context.Savings.FirstOrDefault(x => x.SGID == command.Model.SGID);
            
            if (saving == null)
                throw new SavingNotFoundException("Nie znaleziono oszczędności");
            
            saving.SAmount = command.Model.SAmount;
            saving.STime = command.Model.STime;
            saving.SOnWhat = command.Model.SOnWhat;
            saving.SWhere = command.Model.SWhere;

            context.CreateOrUpdate(saving);  
            context.SaveChanges();
        }
    }
}
