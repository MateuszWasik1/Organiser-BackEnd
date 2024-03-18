using AutoMapper;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class GetSavingsQueryHandler : IQueryHandler<GetSavingsQuery, List<SavingsViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetSavingsQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<SavingsViewModel> Handle(GetSavingsQuery query)
        {
            var savings = context.Savings.OrderBy(x => x.STime).ToList();

            var savingsViewModel = new List<SavingsViewModel>();

            savings.ForEach(x =>
            {
                var sVM = mapper.Map<Cores.Entities.Savings, SavingsViewModel>(x);
                savingsViewModel.Add(sVM);
            });

            return savingsViewModel;
        }
    }
}
