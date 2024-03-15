using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Models.ViewModels.StatsViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public StatsController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetSavingBarChart")]
        public StatsBarChartViewModel GetSavingBarChart(DateTime startDate, DateTime endDate)
            => dispatcher.DispatchQuery<GetSavingBarChartQuery, StatsBarChartViewModel>(new GetSavingBarChartQuery() { StartDate = startDate, EndDate = endDate });

        [HttpGet]
        [Route("GetMoneySpendedFromTaskBarChart")]
        public StatsBarChartViewModel GetMoneySpendedFromTaskBarChart(DateTime startDate, DateTime endDate)
            => dispatcher.DispatchQuery<GetMoneySpendedFromTaskBarChartQuery, StatsBarChartViewModel>(new GetMoneySpendedFromTaskBarChartQuery() { StartDate = startDate, EndDate = endDate });

        [HttpGet]
        [Route("GetMoneySpendedForCategoryBarChart")]
        public StatsBarChartViewModel GetMoneySpendedForCategoryBarChart(DateTime startDate, DateTime endDate, Guid cGID)
            => dispatcher.DispatchQuery<GetMoneySpendedForCategoryBarChartQuery, StatsBarChartViewModel>(new GetMoneySpendedForCategoryBarChartQuery() { StartDate = startDate, EndDate = endDate, CGID = cGID });

        [HttpGet]
        [Route("GetNotesBarChart")]
        public StatsBarChartViewModel GetNotesBarChart(DateTime startDate, DateTime endDate)
            => dispatcher.DispatchQuery<GetNotesBarChartQuery, StatsBarChartViewModel>(new GetNotesBarChartQuery() { StartDate = startDate, EndDate = endDate });
    }
}