using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Roles.Handlers
{
    public class GetIsAnyUserQueryHandler : IQueryHandler<GetIsAnyUserQuery, bool>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public GetIsAnyUserQueryHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public bool Handle(GetIsAnyUserQuery query) {
            var urid = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID;
            return urid == (int) RoleEnum.User || urid == (int) RoleEnum.Premium;
        }
    }
}
