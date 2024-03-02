using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Models.ViewModels;

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
        public List<SavingsViewModel> Get()
            => dispatcher.DispatchQuery<GetSavingsQuery, List<SavingsViewModel>>(new GetSavingsQuery());

        [HttpPost]
        [Route("Save")]
        public void Save(SavingsViewModel model)
            => dispatcher.DispatchCommand(new SaveSavingCommand() { Model = model });

        [HttpDelete]
        [Route("Delete/{sGID}")]
        public void Delete(Guid sGID)
            => dispatcher.DispatchCommand(new DeleteSavingCommand() { SGID = sGID });
    }
}