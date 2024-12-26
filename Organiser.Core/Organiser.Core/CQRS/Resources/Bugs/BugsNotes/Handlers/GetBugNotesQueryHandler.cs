using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Handlers
{
    public class GetBugNotesQueryHandler : IQueryHandler<GetBugNotesQuery, GetBugsNotesViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public GetBugNotesQueryHandler(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        public GetBugsNotesViewModel Handle(GetBugNotesQuery query)
        {
            var bugNotes = new List<Cores.Entities.BugsNotes>();
            var currentUserRole = context.User.AsNoTracking().FirstOrDefault(x => x.UID == user.UID)?.URID ?? (int) RoleEnum.User;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int) RoleEnum.Support)
                bugNotes = context.AllBugsNotes.Where(x => x.BNBGID == query.BGID).OrderBy(x => x.BNDate).AsNoTracking().ToList();
            else
                bugNotes = context.BugsNotes.Where(x => x.BNBGID == query.BGID && !x.BNIsNewVerifier).OrderBy(x => x.BNDate).AsNoTracking().ToList();

            var count = bugNotes.Count;

            bugNotes = bugNotes.Skip(query.Skip).Take(query.Take).ToList();

            var bugNotesViewModel = new List<BugsNotesViewModel>();

            bugNotes.ForEach(x =>
            {
                var bNVM = mapper.Map<Cores.Entities.BugsNotes, BugsNotesViewModel>(x);

                bugNotesViewModel.Add(bNVM);
            });

            var model = new GetBugsNotesViewModel()
            {
                List = bugNotesViewModel,
                Count = count
            };

            return model;
        }
    }
}
