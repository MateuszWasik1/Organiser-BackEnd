using AutoMapper;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers
{
    public class GetBugQueryHandler : IQueryHandler<GetBugQuery, BugViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public GetBugQueryHandler(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        public BugViewModel Handle(GetBugQuery query)
        {
            var bug = new Cores.Entities.Bugs();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int) RoleEnum.Support)
                bug = context.AllBugs.FirstOrDefault(x => x.BGID == query.BGID);
            else
                bug = context.Bugs.FirstOrDefault(x => x.BGID == query.BGID);

            if (bug == null)
                throw new Exception("Nie znaleziono wskazanego błędu !");

            var bugViewModel = mapper.Map<Cores.Entities.Bugs, BugViewModel>(bug);

            return bugViewModel;
        }
    }
}
