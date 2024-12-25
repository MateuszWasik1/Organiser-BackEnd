using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Roles.Handlers
{
    public class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, RolesViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public GetUserRolesQueryHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public RolesViewModel Handle(GetUserRolesQuery query) 
        {
            var userRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? (int) RoleEnum.User;

            var model = new RolesViewModel()
            {
                IsAdmin = userRole == (int) RoleEnum.Admin,
                IsSupport = userRole == (int) RoleEnum.Admin || userRole == (int) RoleEnum.Support,
                IsPremium = userRole == (int) RoleEnum.Admin || userRole == (int) RoleEnum.Support || userRole == (int) RoleEnum.Premium,
                IsUser = userRole == (int) RoleEnum.Admin || userRole == (int) RoleEnum.Support || userRole == (int) RoleEnum.User,
            };

            return model;
        }
    }
}
