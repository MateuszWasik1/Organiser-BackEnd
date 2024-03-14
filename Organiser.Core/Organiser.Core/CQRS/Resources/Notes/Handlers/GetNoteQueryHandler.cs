using AutoMapper;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class GetNoteQueryHandler : IQueryHandler<GetNoteQuery, NotesViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public GetNoteQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public NotesViewModel Handle(GetNoteQuery query)
        {
            var note = context.Notes.FirstOrDefault(x => x.NGID == query.NGID);

            if (note == null)
                throw new Exception("Nie udało znaleźć się notatki!");

            var noteViewModel = mapper.Map<Cores.Entities.Notes, NotesViewModel>(note);

            return noteViewModel;
        }
    }
}
