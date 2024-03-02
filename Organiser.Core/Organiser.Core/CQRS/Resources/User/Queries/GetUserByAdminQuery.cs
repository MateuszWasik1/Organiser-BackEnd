using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Queries
{
    public class GetUserByAdminQuery : IQuery<UserAdminViewModel>
    {
        public Guid UGID { get; set; }
    }
}
