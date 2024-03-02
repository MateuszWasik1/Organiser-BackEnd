using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.CQRS.Resources.Categories.Commands
{
    public class SaveCategoriesCommand : ICommand
    {
        public CategoriesViewModel? Model { get; set; }
    }
}
