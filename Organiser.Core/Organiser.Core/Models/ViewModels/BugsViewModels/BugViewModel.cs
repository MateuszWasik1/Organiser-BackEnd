using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.BugsViewModels
{
    public class BugViewModel
    {
        public Guid BGID { get; set; }
        public string? BTitle { get; set; }
        public string? BText { get; set; }
        public BugStatusEnum? BStatus { get; set; }
    }
}