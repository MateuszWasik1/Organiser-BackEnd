using Organiser.Cores.Models.ViewModels.StatsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Stats.Queries
{
    public class GetMoneySpendedFromTaskBarChartQuery : IQuery<StatsBarChartViewModel>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
