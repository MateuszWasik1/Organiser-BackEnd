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
    public class BugsController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public BugsController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("Get")]
        public List<CategoriesViewModel> Get(DateTime? date)
        {
            var categories = context.Categories.OrderBy(x => x.CStartDate).ToList();

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
    }
}
