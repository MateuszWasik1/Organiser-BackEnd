using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.CQRS.Resources.Categories.Queries
{
    public class GetCategoriesForFilterQuery : IQuery<List<CategoriesForFiltersViewModel>>
    {
    }
}
