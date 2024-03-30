using AutoMapper;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers
{
    public class AddTaskSubTaskCommandHandler : ICommandHandler<AddTaskSubTaskCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public AddTaskSubTaskCommandHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Handle(AddTaskSubTaskCommand command)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.Model.TSTTGID);

            if (task == null)
                throw new TaskNotFoundException("Nie udało się znaleźć podanego zadania!");
        }
    }
}
