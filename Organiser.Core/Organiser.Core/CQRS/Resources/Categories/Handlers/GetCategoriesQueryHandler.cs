using AutoMapper;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Queries;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoriesViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetCategoriesQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<CategoriesViewModel> Handle(GetCategoriesQuery query)
        {
            var categories = context.Categories.OrderBy(x => x.CStartDate).ToList();

            if (query.Date != null)
            {
                var endDate = query.Date.Value.AddMonths(1).AddSeconds(-1);
                categories = categories.Where(x => x.CStartDate >= query.Date && x.CStartDate <= endDate).ToList();
            }

            var categoriesViewModel = new List<CategoriesViewModel>();

            categories.ForEach(x =>
            {
                var cVM = mapper.Map<Cores.Entities.Categories, CategoriesViewModel>(x);
                var tasksBudgetCount = context.Tasks.Where(task => task.TCGID == x.CGID).Sum(x => x.TBudget);
                cVM.CBudgetCount = tasksBudgetCount;

                categoriesViewModel.Add(cVM);
            });

            return categoriesViewModel;
        }
    }
}
