using Organiser.Core.Exceptions.Categories;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Resources.Categories.Commands;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class DeleteCategoriesCommandHandler : ICommandHandler<DeleteCategoriesCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteCategoriesCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteCategoriesCommand command)
        {
            var category = context.Categories.FirstOrDefault(x => command.CGID == x.CGID);

            if (category == null)
                throw new CategoryNotFoundException("Nie znaleziono kategorii");

            var tasksCount = context.Tasks.Where(x => x.TCGID == category.CGID).Count();

            if (tasksCount > 0)
                throw new CategoryHasTasksException($"Nie można usunąć kategorii, do kategorii jest podpięte {tasksCount} zadań");

            context.DeleteCategory(category);
            context.SaveChanges();
        }
    }
}
