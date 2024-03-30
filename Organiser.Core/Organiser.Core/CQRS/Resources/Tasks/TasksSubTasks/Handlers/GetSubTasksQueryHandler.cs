using AutoMapper;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers
{
    public class GetSubTasksQueryHandler : IQueryHandler<GetSubTasksQuery, List<TasksSubTasksViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetSubTasksQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<TasksSubTasksViewModel> Handle(GetSubTasksQuery query)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == query.TGID);

            if (task == null)
                throw new TaskNotFoundException("Nie udało się znaleźć podanego zadania!");

            var subtasks = context.TasksSubTasks.Where(x => x.TSTTGID == query.TGID).OrderBy(x => x.TSTStatus).ThenBy(x => x.TSTModifyDate).ToList();

            var subtasksViewModel = new List<TasksSubTasksViewModel>();

            subtasks.ForEach(x =>
            {
                var tstVM = mapper.Map<Cores.Entities.TasksSubTasks, TasksSubTasksViewModel>(x);

                subtasksViewModel.Add(tstVM);
            });

            return subtasksViewModel;

        }
    }
}
