using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Models.Enums;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries
{
    public class GetBugsQuery : IQuery<List<BugsViewModel>>
    {
        public BugTypeEnum BugType { get; set; }
    }
}
