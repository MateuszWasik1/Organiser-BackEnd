using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.TasksViewModels
{
    public class TasksSubTasksChangeStatusViewModel
    {
        public Guid TSTGID { get; set; }
        public SubTasksStatusEnum Status { get; set; }
    }
}
