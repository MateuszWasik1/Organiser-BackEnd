using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers
{
    public class GetSubTasksQueryHandler : IQueryHandler<GetSubTasksQuery, GetTasksSubTasksViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetSubTasksQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GetTasksSubTasksViewModel Handle(GetSubTasksQuery query)
        {
            var task = context.Tasks.AsNoTracking().FirstOrDefault(x => x.TGID == query.TGID);

            if (task == null)
                throw new TaskNotFoundException("Nie udało się znaleźć podanego zadania!");

            var subtasks = context.TasksSubTasks.Where(x => x.TSTTGID == query.TGID).OrderBy(x => x.TSTStatus).ThenBy(x => x.TSTCreationDate).AsNoTracking().ToList();

            var subtasksViewModel = new List<TasksSubTasksViewModel>();

            var count = subtasks.Count;
            subtasks = subtasks.Skip(query.Skip).Take(query.Take).ToList();

            subtasks.ForEach(x =>
            {
                var tstVM = mapper.Map<Cores.Entities.TasksSubTasks, TasksSubTasksViewModel>(x);

                subtasksViewModel.Add(tstVM);
            });

            var model = new GetTasksSubTasksViewModel()
            {
                List = subtasksViewModel,
                Count = count
            };

            return model;

        }
    }
}
