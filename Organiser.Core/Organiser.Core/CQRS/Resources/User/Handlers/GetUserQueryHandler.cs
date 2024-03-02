using AutoMapper;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserViewModel>
    {

        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public GetUserQueryHandler(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        public UserViewModel Handle(GetUserQuery query)
        {
            var userData = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (userData == null)
                throw new Exception("Nie znaleziono użytkownika!");

            var model = mapper.Map<Cores.Entities.User, UserViewModel>(userData);

            return model;
        }
    }
}
