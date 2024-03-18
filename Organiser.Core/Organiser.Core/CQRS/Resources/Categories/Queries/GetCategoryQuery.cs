using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.CQRS.Resources.Categories.Queries
{
    public class GetCategoryQuery : IQuery<CategoryViewModel>
    {
        public Guid CGID { get; set; }
    }
}
