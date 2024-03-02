using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers
{
    public class AddTaskNoteCommandHandler : ICommandHandler<AddTaskNoteCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddTaskNoteCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddTaskNoteCommand command)
        {
            var taskNote = new Cores.Entities.TasksNotes()
            {
                TNGID = command.Model.TNGID,
                TNTGID = command.Model.TNTGID,
                TNUID = user.UID,
                TNNote = command.Model.TNNote,
                TNDate = DateTime.Now,
            };

            context.CreateOrUpdate(taskNote);

            context.SaveChanges();
        }
    }
}
