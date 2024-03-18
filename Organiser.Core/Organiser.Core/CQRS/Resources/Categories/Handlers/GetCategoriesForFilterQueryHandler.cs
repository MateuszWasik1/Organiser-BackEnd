using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Queries;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class GetCategoriesForFilterQueryHandler : IQueryHandler<GetCategoriesForFilterQuery, List<CategoriesForFiltersViewModel>>
    {
        private readonly IDataBaseContext context;
        public GetCategoriesForFilterQueryHandler(IDataBaseContext context) => this.context = context;

        public List<CategoriesForFiltersViewModel> Handle(GetCategoriesForFilterQuery query)
        {
            var categories = context.Categories.OrderBy(x => x.CStartDate).ToList();

            var viewModel = new List<CategoriesForFiltersViewModel>();

            categories.ForEach(x =>
            {
                var model = new CategoriesForFiltersViewModel()
                {
                    CID = x.CID,
                    CGID = x.CGID,
                    CName = x.CName,
                };

                viewModel.Add(model);
            });

            return viewModel;
        }
    }
}
