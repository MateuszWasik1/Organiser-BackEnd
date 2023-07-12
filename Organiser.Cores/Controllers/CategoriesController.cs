using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public CategoriesController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public List<CategoriesViewModel> Get()
        {
            //ToDo: fix where condition to accept current CUID instead of 1;
            var categories = context.Categories.Where(x => x.CUID == 1).ToList();

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

        [HttpPost]
        [Route("Save")]
        public void Save(CategoriesViewModel model)
        {
            if (model.CID == 0)
            {
                var category = new Categories()
                {
                    CGID = model.CGID,
                    CUID = 1, //poprawić by przekazywało poprawny UID
                    CName = model.CName,
                    CStartDate = model.CStartDate,
                    CEndDate = model.CEndDate,
                    CBudget = model.CBudget,
                };

                context.Categories.Add(category);
            }
            else
            {
                var category = context.Categories.FirstOrDefault(x => x.CGID == model.CGID);

                category.CName = model.CName;
                category.CStartDate = model.CStartDate;
                category.CEndDate = model.CEndDate;
                category.CBudget = model.CBudget;
            }

            context.SaveChanges();
        }
    }
}
