using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Roles.Handlers
{
    public class GetIsUserSupportQueryHandler : IQueryHandler<GetIsUserSupportQuery, bool>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public GetIsUserSupportQueryHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public bool Handle(GetIsUserSupportQuery query) => context.User.FirstOrDefault(x => x.UID == user.UID)?.URID == (int) RoleEnum.Support;

    }
}
