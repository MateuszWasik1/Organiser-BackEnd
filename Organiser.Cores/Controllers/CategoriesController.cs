using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public CategoriesController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("Get")]
        public List<CategoriesViewModel> Get(DateTime? date)
        {
            //ToDo: fix where condition to accept current CUID instead of 1;
            var categories = context.Categories.Where(x => x.CUID == 1).OrderBy(x => x.CStartDate).ToList();

            if (date != null)
            {
                var endDate = date.Value.AddMonths(1).AddSeconds(-1);
                categories = categories.Where(x => x.CStartDate >= date && x.CStartDate <= endDate).ToList();
            }

            var categoriesViewModel = new List<CategoriesViewModel>();

            categories.ForEach(x =>
            {
                var cVM = mapper.Map<Categories, CategoriesViewModel>(x);
                var tasksBudgetCount = context.Tasks.Where(task => task.TCGID == x.CGID).Sum(x => x.TBudget);
                cVM.CBudgetCount = tasksBudgetCount;

                categoriesViewModel.Add(cVM);
            });

            return categoriesViewModel;
        }

        [HttpGet]
        [Route("GetCategoriesForFilter")]
        public List<CategoriesForFiltersViewModel> GetCategoriesForFilter()
        {
            var categories = context.Categories.OrderBy(x => x.CStartDate).ToList();

            var viewModel = new List<CategoriesForFiltersViewModel>();

            categories.ForEach(x =>
            {
                var model = new CategoriesForFiltersViewModel()
                {
                    CID = x.CID,
                    CGID = x.CGID,
                    CName = x.CName,
                };

                viewModel.Add(model);
            });

            return viewModel;
        }

        [HttpPost]
        [Route("Save")]
        public void Save(CategoriesViewModel model)
        {
            if (model.CID == 0)
            {
                var category = new Categories()
                {
                    CGID = model.CGID,
                    CUID = user.UID,
                    CName = model.CName,
                    CStartDate = model.CStartDate,
                    CEndDate = model.CEndDate,
                    CBudget = model.CBudget,
                };

                context.CreateOrUpdate(category);
            }
            else
            {
                var category = context.Categories.FirstOrDefault(x => x.CGID == model.CGID);

                if (category == null)
                    throw new Exception("Nie znaleziono kategorii");

                category.CName = model.CName;
                category.CStartDate = model.CStartDate;
                category.CEndDate = model.CEndDate;
                category.CBudget = model.CBudget;
            }

            context.SaveChanges();
        }

        [HttpDelete]
        [Route("Delete/{cGID}")]
        public void Delete(Guid cGID)
        {
            var category = context.Categories.FirstOrDefault(x => cGID == x.CGID);

            if (category == null)
                throw new Exception("Nie znaleziono kategorii");

            var tasksCount = context.Tasks.Where(x => x.TCGID == category.CGID).Count();

            if (tasksCount > 0)
                throw new Exception($"Nie można usunąć kategorii, do kategorii jest podpięte {tasksCount} zadań");

            context.DeleteCategory(category);
            context.SaveChanges();
        }
    }
}
