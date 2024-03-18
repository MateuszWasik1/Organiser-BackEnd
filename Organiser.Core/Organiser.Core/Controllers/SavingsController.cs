using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavingsController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public SavingsController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetSaving")]
        public SavingViewModel GetSaving(Guid sgid)
            => dispatcher.DispatchQuery<GetSavingQuery, SavingViewModel>(new GetSavingQuery() { SGID = sgid });

        [HttpGet]
        [Route("GetSavings")]
        public List<SavingsViewModel> GetSavings()
            => dispatcher.DispatchQuery<GetSavingsQuery, List<SavingsViewModel>>(new GetSavingsQuery());

        [HttpPost]
        [Route("AddSaving")]
        public void AddSaving(SavingViewModel model)
            => dispatcher.DispatchCommand(new AddSavingCommand() { Model = model });

        [HttpPut]
        [Route("UpdateSaving")]
        public void UpdateSaving(SavingViewModel model)
            => dispatcher.DispatchCommand(new UpdateSavingCommand() { Model = model });

        [HttpDelete]
        [Route("Delete/{sGID}")]
        public void Delete(Guid sGID)
            => dispatcher.DispatchCommand(new DeleteSavingCommand() { SGID = sGID });
    }
}