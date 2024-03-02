using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;
using System.ComponentModel.Design;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class DeleteSavingCommandHandler : ICommandHandler<DeleteSavingCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteSavingCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteSavingCommand command) 
        {
            var saving = context.Savings.FirstOrDefault(x => command.SGID == x.SGID);

            if (saving == null)
                throw new Exception("Nie znaleziono oszczędności");

            context.DeleteSaving(saving);
            context.SaveChanges();
        }
    }
}
