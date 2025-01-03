﻿using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Helpers;
using Organiser.Cores.Models.ViewModels.StatsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Stats.Handlers
{
    public class GetMoneySpendedFromTaskBarChartQueryHandler : IQueryHandler<GetMoneySpendedFromTaskBarChartQuery, StatsBarChartViewModel>
    {
        private readonly IDataBaseContext context;
        public GetMoneySpendedFromTaskBarChartQueryHandler(IDataBaseContext context) => this.context = context;

        public StatsBarChartViewModel Handle(GetMoneySpendedFromTaskBarChartQuery query)
        {
            var tasksForPeriod = context.Tasks
                .Where(x => query.StartDate <= x.TTime && x.TTime <= query.EndDate)
                .OrderBy(x => x.TTime)
                .AsNoTracking()
                .ToList();

            var timeSpanBetweenStartAndEndDate = MonthsBetweenDatesHelper.MonthsBetween(query.StartDate, query.EndDate);

            var data = new StatsBarChartViewModel()
            {
                Labels = new List<string>(),
                Datasets = new ChartDatasetViewModel(),
            };

            var model = new ChartDatasetViewModel()
            {
                Label = $"Wydane pieniądze na zadania",
                Data = new List<decimal>(),
            };

            foreach (var x in timeSpanBetweenStartAndEndDate)
            {
                var month = new DateTime(x.Year, x.Month, 1);
                var nextMonth = month.AddMonths(1).AddSeconds(-1);

                if (query.StartDate.Year == x.Year && query.StartDate.Month == x.Month)
                {
                    month = new DateTime(x.Year, x.Month, query.StartDate.Day);
                    nextMonth = month.AddMonths(1).AddDays(-query.StartDate.Day + 1).AddSeconds(-1);
                }
                if (query.EndDate.Year == x.Year && query.EndDate.Month == x.Month)
                    nextMonth = new DateTime(x.Year, x.Month, query.EndDate.Day, 23, 59, 59);

                var tasksMoneySpendedForMonth = tasksForPeriod.Where(x => month <= x.TTime && x.TTime <= nextMonth).Sum(x => x.TBudget);

                model?.Data?.Add(tasksMoneySpendedForMonth);

                data?.Labels?.Add($"{x.Year}-{x.Month}");
            }

            data.Datasets = model;

            return data;
        }
    }
}
