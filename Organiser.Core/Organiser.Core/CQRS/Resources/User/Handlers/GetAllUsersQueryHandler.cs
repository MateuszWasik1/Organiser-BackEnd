using AutoMapper;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, List<UsersAdminViewModel>>
    {

        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetAllUsersQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<UsersAdminViewModel> Handle(GetAllUsersQuery query)
        {
            var usersData = context.AllUsers.ToList();

            var usersAdmViewModel = new List<UsersAdminViewModel>();

            usersData.ForEach(x => {
                var model = mapper.Map<Cores.Entities.User, UsersAdminViewModel>(x);
                usersAdmViewModel.Add(model);
            });

            return usersAdmViewModel;
        }
    }
}
