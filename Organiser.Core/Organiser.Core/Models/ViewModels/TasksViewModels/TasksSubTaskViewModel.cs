using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.TasksViewModels
{
    public class TasksSubTaskViewModel
    {
        public string? TSTTitle { get; set; }
        public string? TSTNext { get; set; }
        public DateTime TSTCreationDate { get; set; }
        public DateTime? TSTModifyDate { get; set; }
        public SubTasksStatusEnum TSTStatus { get; set; }
    }
}