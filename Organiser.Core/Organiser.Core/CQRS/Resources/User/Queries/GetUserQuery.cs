using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Queries
{
    public class GetUserQuery : IQuery<UserViewModel>
    {
    }
}
