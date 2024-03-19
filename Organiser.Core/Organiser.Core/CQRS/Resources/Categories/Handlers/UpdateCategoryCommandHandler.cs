using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Resources.Categories.Commands;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly IDataBaseContext context;
        public UpdateCategoryCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(UpdateCategoryCommand command)
        {
            var category = context.Categories.FirstOrDefault(x => x.CGID == command.Model.CGID);
            
            if (category == null)
                throw new Exception("Nie znaleziono kategorii");
            
            category.CName = command.Model.CName;
            category.CStartDate = command.Model.CStartDate;
            category.CEndDate = command.Model.CEndDate;
            category.CBudget = command.Model.CBudget;
            
            context.CreateOrUpdate(category);
            context.SaveChanges();
        }
    }
}