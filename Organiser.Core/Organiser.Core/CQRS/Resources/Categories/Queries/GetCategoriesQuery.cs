using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.CQRS.Resources.Categories.Queries
{
    public class GetCategoriesQuery : IQuery<GetCategoriesViewModel>
    {
        public DateTime? Date { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
