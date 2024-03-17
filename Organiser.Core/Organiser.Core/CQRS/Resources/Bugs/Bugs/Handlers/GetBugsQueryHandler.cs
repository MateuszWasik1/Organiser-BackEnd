using AutoMapper;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers
{
    public class GetBugsQueryHandler : IQueryHandler<GetBugsQuery, List<BugsViewModel>>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public GetBugsQueryHandler(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        public List<BugsViewModel> Handle(GetBugsQuery query)
        {
            var bugs = new List<Cores.Entities.Bugs>();
            var bugsViewModel = new List<BugsViewModel>();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int) RoleEnum.Support)
            {
                if (query.BugType == BugTypeEnum.My)
                    bugs = context.AllBugs.Where(x => x.BUID == user.UID).OrderBy(x => x.BDate).ToList();

                else if (query.BugType == BugTypeEnum.ImVerificator)
                    bugs = context.AllBugs.Where(x => x.BAUIDS.Contains(user.UGID)).OrderBy(x => x.BDate).ToList();

                else if (query.BugType == BugTypeEnum.Closed)
                    bugs = context.AllBugs.Where(x => x.BStatus == BugStatusEnum.Rejected || x.BStatus == BugStatusEnum.Fixed).OrderBy(x => x.BDate).ToList();

                else if(query.BugType == BugTypeEnum.New)
                    bugs = context.AllBugs.Where(x => x.BStatus == BugStatusEnum.New).OrderBy(x => x.BDate).ToList();

                else if(query.BugType == BugTypeEnum.All)
                    bugs = context.AllBugs.OrderBy(x => x.BDate).ToList();

                else
                    bugs = context.AllBugs.OrderBy(x => x.BDate).ToList();

                var supportGIDs = bugs.Where(x => x.BAUIDS != null).SelectMany(x => x.BAUIDS?.Split(',')).Distinct().ToList();
                var supportUsers = context.AllUsers.Where(x => supportGIDs.Contains(x.UGID.ToString())).Select(x => new { x.UFirstName, x.ULastName, x.UGID }).ToList();

                bugs.ForEach(x =>
                {
                    var bVM = mapper.Map<Cores.Entities.Bugs, BugsViewModel>(x);

                    if (x.BAUIDS != null)
                    {
                        var bugSupportUsers = supportUsers.Where(u => x.BAUIDS.Contains(u.UGID.ToString())).ToList();
                        bugSupportUsers.ForEach(u => bVM.BVerifiers += $"{u.UFirstName} {u.ULastName} {u.UGID} ");
                    }

                    bugsViewModel.Add(bVM);
                });
            }

            else
            {
                bugs = context.Bugs.OrderBy(x => x.BDate).ToList();

                bugs.ForEach(x =>
                {
                    var bVM = mapper.Map<Cores.Entities.Bugs, BugsViewModel>(x);

                    bugsViewModel.Add(bVM);
                });
            }

            return bugsViewModel;
        }
    }
}
