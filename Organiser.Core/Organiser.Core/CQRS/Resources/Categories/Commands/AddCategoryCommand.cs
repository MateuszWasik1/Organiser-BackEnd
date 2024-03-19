using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.CQRS.Resources.Categories.Commands
{
    public class AddCategoryCommand : ICommand
    {
        public CategoryViewModel? Model { get; set; }
    }
}
