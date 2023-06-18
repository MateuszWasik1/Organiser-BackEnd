using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
                categoriesViewModel.Add(mapper.Map<CategoriesViewModel>(x));

            });

            return categoriesViewModel;
        }

        // GET api/<TestController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<TestController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<TestController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TestController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
