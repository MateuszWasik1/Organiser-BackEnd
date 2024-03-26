using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.TasksViewModels
{
    public class TasksSubTasksViewModel
    {
        public Guid TSTGID { get; set; }
        public string? TSTTitle { get; set; }
        public string? TSTNext { get; set; }
        public SubTasksStatusEnum TSTStatus { get; set; }
    }
}