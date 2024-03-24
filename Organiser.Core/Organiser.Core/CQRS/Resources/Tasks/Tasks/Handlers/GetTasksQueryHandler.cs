﻿using AutoMapper;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, List<TasksViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetTasksQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<TasksViewModel> Handle(GetTasksQuery query)
        {
            List<Cores.Entities.Tasks> tasks = context.Tasks.OrderBy(x => x.TTime).ToList();

            if (!string.IsNullOrEmpty(query.CGID))
                tasks = tasks.Where(x => x.TCGID == Guid.Parse(query.CGID)).ToList();

            if (query.Status != 3)
                tasks = tasks.Where(x => (int) x.TStatus == query.Status).ToList();

            var tasksViewModel = new List<TasksViewModel>();

            tasks.ForEach(x =>
            {
                var tVM = mapper.Map<Cores.Entities.Tasks, TasksViewModel>(x);

                tasksViewModel.Add(tVM);
            });

            return tasksViewModel;
        }
    }
}
