using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries
{
    public class GetBugQuery : IQuery<BugViewModel>
    {
        public Guid BGID { get; set; }
    }
}
