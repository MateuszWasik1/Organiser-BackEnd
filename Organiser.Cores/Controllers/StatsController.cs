using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Helpers;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Models.ViewModels.StatsViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IDataBaseContext context;
        public StatsController(IDataBaseContext context) => this.context = context;

        [HttpGet]
        [Route("GetSavingBarChart")]
        public SavingStatsBarChartViewModel GetSavingBarChart(DateTime startDate, DateTime endDate)
        {
            var savingsForPeriod = context.Savings.Where(x => startDate <= x.STime && x.STime <= endDate).OrderBy(x => x.STime).ToList();

            var timeSpanBetweenStartAndEndDate = MonthsBetweenDatesHelper.MonthsBetween(startDate, endDate);

            var data = new SavingStatsBarChartViewModel()
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
                var nextMonth = month.AddDays(1).AddSeconds(-1);

                if (startDate.Year == x.Year && startDate.Month == x.Month)
                {
                    month = new DateTime(x.Year, x.Month, startDate.Day);
                    nextMonth = month.AddMonths(1).AddDays(- startDate.Day + 1).AddSeconds(-1);
                }
                if (endDate.Year == x.Year && endDate.Month == x.Month)
                    nextMonth = new DateTime(x.Year, x.Month, endDate.Day, 23, 59, 59);

                var savingsForMonth = savingsForPeriod.Where(x => month <= x.STime && x.STime <= nextMonth).Sum(x => x.SAmount);

                model?.Data?.Add(savingsForMonth);

                data?.Labels?.Add($"{x.Year}-{x.Month}");
            }

            data.Datasets = model;

            return data;
        }
    }
}