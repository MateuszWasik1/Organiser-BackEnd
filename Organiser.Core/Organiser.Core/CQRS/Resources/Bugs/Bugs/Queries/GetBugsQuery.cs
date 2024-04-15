using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Models.Enums;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries
{
    public class GetBugsQuery : IQuery<GetBugsViewModel>
    {
        public BugTypeEnum BugType { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
