using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Resources.Categories.Commands;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class SaveCategoriesCommandHandler : ICommandHandler<SaveCategoriesCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public SaveCategoriesCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(SaveCategoriesCommand command)
        {
            if (command?.Model?.CID == 0)
            {
                var category = new Cores.Entities.Categories()
                {
                    CGID = command.Model.CGID,
                    CUID = user.UID,
                    CName = command.Model.CName,
                    CStartDate = command.Model.CStartDate,
                    CEndDate = command.Model.CEndDate,
                    CBudget = command.Model.CBudget,
                };

                context.CreateOrUpdate(category);
            }
            else
            {
                var category = context.Categories.FirstOrDefault(x => x.CGID == command.Model.CGID);

                if (category == null)
                    throw new Exception("Nie znaleziono kategorii");

                category.CName = command.Model.CName;
                category.CStartDate = command.Model.CStartDate;
                category.CEndDate = command.Model.CEndDate;
                category.CBudget = command.Model.CBudget;
            }

            context.SaveChanges();
        }
    }
}
