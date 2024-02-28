﻿using AutoMapper;
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
    public class TasksController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public TasksController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        public List<TasksViewModel> Get(string cGID = "", int status = 3)
        {
            var tasks = context.Tasks.OrderBy(x => x.TTime).ToList();

            if(!string.IsNullOrEmpty(cGID))
                tasks = tasks.Where(x => x.TCGID == Guid.Parse(cGID)).ToList();

            if(status != 3)
                tasks = tasks.Where(x => (int) x.TStatus == status).ToList();

            var tasksViewModel = new List<TasksViewModel>();

            tasks.ForEach(x =>
            {
                tasksViewModel.Add(mapper.Map<Tasks, TasksViewModel>(x));
            });

            return tasksViewModel;
        }

        [HttpPost]
        [Route("Save")]
        public void Save(TasksViewModel model)
        {
            if (model.TID == 0)
            {
                var task = new Tasks()
                {
                    TGID = model.TGID,
                    TUID = user.UID,
                    TCGID = model.TCGID,
                    TName = model.TName,
                    TLocalization = model.TLocalization,
                    TTime = model.TTime,
                    TBudget = model.TBudget,
                    TStatus = model.TStatus,
                };

                context.CreateOrUpdate(task);
            }
            else
            {
                var task = context.Tasks.FirstOrDefault(x => x.TGID == model.TGID);

                if (task == null)
                    throw new Exception("Nie znaleziono zadania");

                task.TCGID = model.TCGID;
                task.TName = model.TName;
                task.TLocalization = model.TLocalization;
                task.TTime = model.TTime;
                task.TBudget = model.TBudget;
                task.TStatus = model.TStatus;
            }

            context.SaveChanges();
        }

        [HttpDelete]
        [Route("Delete/{tGID}")]
        public void Delete(Guid tGID)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == tGID);

            if(task == null)
                throw new Exception("Nie znaleziono zadania");

            context.DeleteTask(task);
            context.SaveChanges();
        }
    }
}