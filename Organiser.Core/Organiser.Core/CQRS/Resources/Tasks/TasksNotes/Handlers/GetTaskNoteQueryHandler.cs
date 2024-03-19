using AutoMapper;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers
{
    public class GetTaskNoteQueryHandler : IQueryHandler<GetTaskNoteQuery, List<TasksNotesViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetTaskNoteQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<TasksNotesViewModel> Handle(GetTaskNoteQuery query)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == query.TGID);

            if (task == null)
                throw new Exception("Nie znaleziono notatki do zadania. Nie można załadować notatek!");

            var taskNotes = context.TasksNotes.Where(x => x.TNTGID == task.TGID).OrderBy(x => x.TNDate).ToList();

            var taskNotesViewModel = new List<TasksNotesViewModel>();

            taskNotes.ForEach(x =>
            {
                taskNotesViewModel.Add(mapper.Map<Cores.Entities.TasksNotes, TasksNotesViewModel>(x));
            });

            return taskNotesViewModel;
        }
    }
}
