using Organiser.Core.Exceptions.Categories;
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
            if (command.Model.CName.Length == 0)
                throw new CategoryNameRequiredException("Nazwa kategorii nie może być pusta!");

            if (command.Model.CName.Length > 300)
                throw new CategoryNameMax300Exception("Nazwa kategorii nie może przekraczać 300 znaków!");

            if (command.Model.CBudget < 0)
                throw new CategoryBudgetMin0Exception("Budżet nie może być mniejszy od zera!");

            var category = context.Categories.FirstOrDefault(x => x.CGID == command.Model.CGID);
            
            if (category == null)
                throw new CategoryNotFoundException("Nie znaleziono kategorii");
            
            category.CName = command.Model.CName;
            category.CStartDate = command.Model.CStartDate;
            category.CEndDate = command.Model.CEndDate;
            category.CBudget = command.Model.CBudget;
            
            context.CreateOrUpdate(category);
            context.SaveChanges();
        }
    }
}