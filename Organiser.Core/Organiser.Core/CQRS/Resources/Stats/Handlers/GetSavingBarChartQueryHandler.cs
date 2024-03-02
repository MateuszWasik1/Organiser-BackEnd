using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Helpers;
using Organiser.Cores.Models.ViewModels.StatsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Stats.Handlers
{
    public class GetSavingBarChartQueryHandler : IQueryHandler<GetSavingBarChartQuery, StatsBarChartViewModel>
    {
        private readonly IDataBaseContext context;
        public GetSavingBarChartQueryHandler(IDataBaseContext context) => this.context = context;

        public StatsBarChartViewModel Handle(GetSavingBarChartQuery query)
        {
            var savingsForPeriod = context.Savings.Where(x => query.StartDate <= x.STime && x.STime <= query.EndDate).OrderBy(x => x.STime).ToList();

            var timeSpanBetweenStartAndEndDate = MonthsBetweenDatesHelper.MonthsBetween(query.StartDate, query.EndDate);

            var data = new StatsBarChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new ChartDatasetViewModel(),
            };

            var model = new ChartDatasetViewModel()
            {
                Label = $"Oszczędności",
                Data = new List<decimal>(),
            };

            foreach (var x in timeSpanBetweenStartAndEndDate)
            {
                var month = new DateTime(x.Year, x.Month, 1);
                var nextMonth = month.AddMonths(1).AddSeconds(-1);

                if (query.StartDate.Year == x.Year && query.StartDate.Month == x.Month)
                {
                    month = new DateTime(x.Year, x.Month, query.StartDate.Day);
                    nextMonth = month.AddMonths(1).AddDays(- query.StartDate.Day + 1).AddSeconds(-1);
                }
                if (query.EndDate.Year == x.Year && query.EndDate.Month == x.Month)
                    nextMonth = new DateTime(x.Year, x.Month, query.EndDate.Day, 23, 59, 59);

                var savingsForMonth = savingsForPeriod.Where(x => month <= x.STime && x.STime <= nextMonth).Sum(x => x.SAmount);

                model?.Data?.Add(savingsForMonth);

                data?.Labels?.Add($"{x.Year}-{x.Month}");
            }

            data.Datasets = model;

            return data;
        }
    }
}
