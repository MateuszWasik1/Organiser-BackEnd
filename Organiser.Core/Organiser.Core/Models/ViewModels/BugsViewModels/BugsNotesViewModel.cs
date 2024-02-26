using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.BugsViewModels
{
    public class BugsNotesViewModel
    {
        public Guid BNGID { get; set; }
        public Guid BNBGID { get; set; }
        public DateTime BNDate { get; set; }
        public string? BNText { get; set; }
        public bool BNIsNewVerifier { get; set; }
        public bool BNIsStatusChange { get; set; }
        public BugStatusEnum BNChangedStatus { get; set; }
    }
}