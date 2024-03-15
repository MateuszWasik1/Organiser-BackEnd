using AutoMapper;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class GetNotesQueryHandler : IQueryHandler<GetNotesQuery, List<NotesViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public GetNotesQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<NotesViewModel> Handle(GetNotesQuery query)
        {
            var notes = context.Notes.ToList();

            var notesViewModel = new List<NotesViewModel>();

            notes.ForEach(x =>
            {
                var nVM = mapper.Map<Cores.Entities.Notes, NotesViewModel>(x);

                notesViewModel.Add(nVM);
            });

            return notesViewModel;
        }
    }
}
