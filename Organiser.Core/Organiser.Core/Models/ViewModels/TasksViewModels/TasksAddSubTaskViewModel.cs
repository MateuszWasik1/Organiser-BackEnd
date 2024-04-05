namespace Organiser.Core.Models.ViewModels.TasksViewModels
{
    public class TasksAddSubTaskViewModel
    {
        public Guid TSTGID { get; set; }
        public Guid TSTTGID { get; set; }
        public string? TSTTitle { get; set; }
        public string? TSTText { get; set; }
    }
}