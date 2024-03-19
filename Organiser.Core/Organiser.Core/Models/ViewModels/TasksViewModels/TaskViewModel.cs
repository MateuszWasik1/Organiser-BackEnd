using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.TasksViewModels
{
    public class TaskViewModel
    {
        public Guid TGID { get; set; }
        public Guid TCGID { get; set; }
        public string? TName { get; set; }
        public string? TLocalization { get; set; }
        public DateTime TTime { get; set; }
        public int TBudget { get; set; }
        public TaskEnum TStatus { get; set; }
    }
}
