using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.BugsViewModels
{
    public class ChangeBugStatusViewModel
    {
        public Guid BGID { get; set; }
        public BugStatusEnum Status { get; set; }
    }
}