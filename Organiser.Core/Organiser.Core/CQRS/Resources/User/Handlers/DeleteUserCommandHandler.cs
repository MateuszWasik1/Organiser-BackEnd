using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.Exceptions;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IDataBaseContext context;
        public DeleteUserCommandHandler(IDataBaseContext context) => this.context = context;

        public void Handle(DeleteUserCommand command)
        {
            var deletedUser = context.AllUsers.FirstOrDefault(x => x.UGID == command.UGID);

            if (deletedUser == null)
                throw new UserNotFoundExceptions("Nie znaleziono użytkownika!");

            var categories = context.AllCategories.Where(x => x.CUID == deletedUser.UID).ToList();
            var tasks = context.AllTasks.Where(x => x.TUID == deletedUser.UID).ToList();
            var taskNotes = context.AllTasksNotes.Where(x => x.TNUID == deletedUser.UID).ToList();
            var taskSubTasks = context.AllTasksSubTasks.Where(x => x.TSTUID == deletedUser.UID).ToList();
            var savings = context.AllSavings.Where(x => x.SUID == deletedUser.UID).ToList();
            var notes = context.AllNotes.Where(x => x.NUID == deletedUser.UID).ToList();

            foreach (var category in categories)
                context.DeleteCategory(category);

            foreach (var task in tasks)
                context.DeleteTask(task);

            foreach (var taskNote in taskNotes)
                context.DeleteTaskNotes(taskNote);

            foreach (var taskSubTask in taskSubTasks)
                context.DeleteTaskSubTask(taskSubTask);

            foreach (var saving in savings)
                context.DeleteSaving(saving);

            foreach (var note in notes)
                context.DeleteNote(note);

            context.DeleteUser(deletedUser);
            context.SaveChanges();
        }
    }
}
