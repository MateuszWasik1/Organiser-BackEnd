using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Resources.Categories.Commands;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public CategoriesController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("Get")]
        public List<CategoriesViewModel> Get(DateTime? date) => dispatcher.DispatchQuery<GetCategoriesQuery, List<CategoriesViewModel>>(new GetCategoriesQuery() { Date = date });

        [HttpGet]
        [Route("GetCategoriesForFilter")]
        public List<CategoriesForFiltersViewModel> GetCategoriesForFilter() => dispatcher.DispatchQuery<GetCategoriesForFilterQuery, List<CategoriesForFiltersViewModel>>(new GetCategoriesForFilterQuery());

        [HttpPost]
        [Route("Save")]
        public void Save(CategoriesViewModel model) => dispatcher.DispatchCommand(new SaveCategoriesCommand() { Model = model });

        [HttpDelete]
        [Route("Delete/{cGID}")]
        public void Delete(Guid cGID) => dispatcher.DispatchCommand(new DeleteCategoriesCommand() { CGID = cGID });
    }
}