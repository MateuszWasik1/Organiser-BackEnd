using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Entities;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DataContext context;

        public TestController(DataContext context)
        {
            this.context = context;
        }

        // GET: api/<TestController>
        [HttpGet]
        public List<TestDB> Get()
        {
            //return new List<TestDB>()
            //{
            //    new TestDB()
            //    {
            //        ID = 1,
            //        Name = "test",
            //        vary = true
            //    }
            //};
            return context.TestDBs.ToList();
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
