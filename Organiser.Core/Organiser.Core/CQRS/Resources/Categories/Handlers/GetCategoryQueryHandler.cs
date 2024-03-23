using AutoMapper;
using Organiser.Core.Exceptions.Categories;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Abstraction.Queries;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class GetCategoryQueryHandler : IQueryHandler<GetCategoryQuery, CategoryViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetCategoryQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public CategoryViewModel Handle(GetCategoryQuery query)
        {
            var category = context.Categories.FirstOrDefault(x => x.CGID == query.CGID);

            if (category == null)
                throw new CategoryNotFoundException("Nie udało się znaleźć kategorii");

            var categoryViewModel = mapper.Map<Cores.Entities.Categories, CategoryViewModel>(category);

            var tasksBudgetCount = context.Tasks.Where(task => task.TCGID == category.CGID).Sum(x => x.TBudget);

            categoryViewModel.CBudgetCount = tasksBudgetCount;

            return categoryViewModel;
        }
    }
}