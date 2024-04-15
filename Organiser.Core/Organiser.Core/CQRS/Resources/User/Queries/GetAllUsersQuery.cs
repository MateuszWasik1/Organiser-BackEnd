using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Queries
{
    public class GetAllUsersQuery : IQuery<GetUsersAdminViewModel>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
