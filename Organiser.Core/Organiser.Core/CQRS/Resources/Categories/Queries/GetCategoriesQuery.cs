using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.CQRS.Resources.Categories.Queries
{
    public class GetCategoriesQuery : IQuery<List<CategoriesViewModel>>
    {
        public DateTime? Date { get; set; }
    }
}
