using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers
{
    public class AddTaskSubTaskCommandHandler : ICommandHandler<AddTaskSubTaskCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddTaskSubTaskCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddTaskSubTaskCommand command)
        {
            if (command.Model.TSTTitle.Length == 0)
                throw new TaskSubTaskTitleRequiredException("Tytuł podzadania jest wymagana!");

            if (command.Model.TSTTitle.Length > 200)
                throw new TaskSubTaskTitleMax200Exception("Tytuł podzadania nie może być dłuższy niż 200 znaków!");

            if (command.Model.TSTText.Length == 0)
                throw new TaskSubTaskTextRequiredException("Tytuł podzadania jest wymagana!");

            if (command.Model.TSTText.Length > 2000)
                throw new TaskSubTaskTextMax2000Exception("Tytuł podzadania nie może być dłuższy niż 200 znaków!");

            var task = context.Tasks.FirstOrDefault(x => x.TGID == command.Model.TSTTGID);

            if (task == null)
                throw new TaskNotFoundException("Nie udało się znaleźć podanego zadania!");

            var subtask = new Cores.Entities.TasksSubTasks()
            {
                TSTGID = command.Model.TSTGID,
                TSTTGID = command.Model.TSTTGID,
                TSTUID = user.UID,
                TSTTitle = command.Model.TSTTitle,
                TSTText = command.Model.TSTText,
                TSTCreationDate = DateTime.Now,
                TSTStatus = SubTasksStatusEnum.NotStarted,
            };

            context.CreateOrUpdate(subtask);
            context.SaveChanges();
        }
    }
}
