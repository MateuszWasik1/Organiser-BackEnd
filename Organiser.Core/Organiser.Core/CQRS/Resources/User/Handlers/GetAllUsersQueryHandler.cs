using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetUsersAdminViewModel>
    {

        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetAllUsersQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GetUsersAdminViewModel Handle(GetAllUsersQuery query)
        {
            var usersData = context.AllUsers.AsNoTracking().ToList();

            var usersAdmViewModel = new List<UsersAdminViewModel>();

            var count = usersData.Count;
            usersData = usersData.Skip(query.Skip).Take(query.Take).ToList();

            usersData.ForEach(x => {
                var model = mapper.Map<Cores.Entities.User, UsersAdminViewModel>(x);
                usersAdmViewModel.Add(model);
            });

            var model = new GetUsersAdminViewModel()
            {
                List = usersAdmViewModel,
                Count = count,
            };

            return model;
        }
    }
}
