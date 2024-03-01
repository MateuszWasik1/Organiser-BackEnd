using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.CQRS.Resources.Categories.Commands
{
    public class DeleteCategoriesCommand : ICommand
    {
        public Guid CGID { get; set; }
    }
}
