using AutoMapper;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class GetSavingQueryHandler : IQueryHandler<GetSavingQuery, SavingViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetSavingQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public SavingViewModel Handle(GetSavingQuery query)
        {
            var saving = context.Savings.FirstOrDefault(x => x.SGID == query.SGID);

            if (saving == null)
                throw new Exception("Nie udało się znaleźć oszczędności!");

            var savingViewModel = mapper.Map<Cores.Entities.Savings, SavingViewModel>(saving);

            return savingViewModel;
        }
    }
}
