using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.CQRS.Resources.Categories.Commands
{
    public class UpdateCategoryCommand : ICommand
    {
        public CategoryViewModel? Model { get; set; }
    }
}
