﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IDispatcher dispatcher;
        public CategoriesController(IDataBaseContext context, IUserContext user, IDispatcher dispatcher)
        {
            this.context = context;
            this.user = user;
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        [Route("Get")]
        public List<CategoriesViewModel> Get(DateTime? date) => dispatcher.DispatchQuery<GetCategoriesQuery, List<CategoriesViewModel>>(new GetCategoriesQuery() { Date = date });

        [HttpGet]
        [Route("GetCategoriesForFilter")]
        public List<CategoriesForFiltersViewModel> GetCategoriesForFilter() => dispatcher.DispatchQuery<GetCategoriesForFilterQuery, List<CategoriesForFiltersViewModel>>(new GetCategoriesForFilterQuery());

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
