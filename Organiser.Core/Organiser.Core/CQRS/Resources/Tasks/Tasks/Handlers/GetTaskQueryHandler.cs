using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers
{
    public class GetTaskQueryHandler : IQueryHandler<GetTaskQuery, TaskViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetTaskQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public TaskViewModel Handle(GetTaskQuery query)
        {
            var task = context.Tasks.AsNoTracking().FirstOrDefault(x => x.TGID == query.TGID);

            if (task == null)
                throw new TaskNotFoundException("Nie udało się znaleźć zadania!");

            var taskViewModel = mapper.Map<Cores.Entities.Tasks, TaskViewModel>(task);

            return taskViewModel;
        }
    }
}
