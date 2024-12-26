using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, GetTasksViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetTasksQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GetTasksViewModel Handle(GetTasksQuery query)
        {
           var tasks = context.Tasks.OrderBy(x => x.TTime).AsNoTracking().ToList();

            if (!string.IsNullOrEmpty(query.CGID))
                tasks = tasks.Where(x => x.TCGID == Guid.Parse(query.CGID)).ToList();

            if (query.Status != 3)
                tasks = tasks.Where(x => (int) x.TStatus == query.Status).ToList();

            var tasksViewModel = new List<TasksViewModel>();

            var count = tasks.Count;
            tasks = tasks.Skip(query.Skip).Take(query.Take).ToList();

            tasks.ForEach(x =>
            {
                var tVM = mapper.Map<Cores.Entities.Tasks, TasksViewModel>(x);

                tasksViewModel.Add(tVM);
            });

            var model = new GetTasksViewModel()
            {
                List = tasksViewModel,
                Count = count
            };

            return model;
        }
    }
}
