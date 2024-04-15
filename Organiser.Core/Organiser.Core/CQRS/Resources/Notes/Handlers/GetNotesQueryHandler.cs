using AutoMapper;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class GetNotesQueryHandler : IQueryHandler<GetNotesQuery, GetNotesViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public GetNotesQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GetNotesViewModel Handle(GetNotesQuery query)
        {
            var notes = context.Notes.ToList();

            var notesViewModel = new List<NotesViewModel>();

            var count = notes.Count;
            notes = notes.Skip(query.Skip).Take(query.Take).ToList();

            notes.ForEach(x =>
            {
                var nVM = mapper.Map<Cores.Entities.Notes, NotesViewModel>(x);

                notesViewModel.Add(nVM);
            });

            var model = new GetNotesViewModel()
            {
                List = notesViewModel,
                Count = count
            };

            return model;
        }
    }
}
