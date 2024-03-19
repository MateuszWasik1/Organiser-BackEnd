using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
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
        [Route("GetCategory")]
        public CategoryViewModel GetCategory(Guid cgid)
            => dispatcher.DispatchQuery<GetCategoryQuery, CategoryViewModel>(new GetCategoryQuery() { CGID = cgid });

        [HttpGet]
        [Route("GetCategories")]
        public List<CategoriesViewModel> GetCategories(DateTime? date) 
            => dispatcher.DispatchQuery<GetCategoriesQuery, List<CategoriesViewModel>>(new GetCategoriesQuery() { Date = date });

        [HttpGet]
        [Route("GetCategoriesForFilter")]
        public List<CategoriesForFiltersViewModel> GetCategoriesForFilter() 
            => dispatcher.DispatchQuery<GetCategoriesForFilterQuery, List<CategoriesForFiltersViewModel>>(new GetCategoriesForFilterQuery());

        [HttpPost]
        [Route("AddCategory")]
        public void AddCategory(CategoryViewModel model) 
            => dispatcher.DispatchCommand(new AddCategoryCommand() { Model = model });

        [HttpPut]
        [Route("UpdateCategory")]
        public void UpdateCategory(CategoryViewModel model)
            => dispatcher.DispatchCommand(new UpdateCategoryCommand() { Model = model });

        [HttpDelete]
        [Route("Delete/{cGID}")]
        public void Delete(Guid cGID) 
            => dispatcher.DispatchCommand(new DeleteCategoriesCommand() { CGID = cGID });
    }
}