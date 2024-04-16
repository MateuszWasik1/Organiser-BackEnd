using AutoMapper;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
using Organiser.Core.Exceptions.Notes;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers
{
    public class GetTaskNoteQueryHandler : IQueryHandler<GetTaskNoteQuery, GetTasksNotesViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetTaskNoteQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GetTasksNotesViewModel Handle(GetTaskNoteQuery query)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == query.TGID);

            if (task == null)
                throw new NoteNotFoundException("Nie znaleziono zadania. Nie można załadować notatek!");

            var taskNotes = context.TasksNotes.Where(x => x.TNTGID == task.TGID).OrderBy(x => x.TNDate).ToList();

            var taskNotesViewModel = new List<TasksNotesViewModel>();

            var count = taskNotes.Count;
            taskNotes = taskNotes.Skip(query.Skip).Take(query.Take).ToList();

            taskNotes.ForEach(x =>
            {
                taskNotesViewModel.Add(mapper.Map<Cores.Entities.TasksNotes, TasksNotesViewModel>(x));
            });

            var model = new GetTasksNotesViewModel()
            {
                List = taskNotesViewModel,
                Count = count
            };

            return model;
        }
    }
}
