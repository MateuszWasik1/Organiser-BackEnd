namespace Organiser.Core.Models.ViewModels.TasksViewModels
{
    public class TasksDeleteTaskRelatedEntitiesViewModel
    {
        public Guid TGID { get; set; }
        public bool DeleteTaskSubTasks { get; set; }
        public bool DeleteTaskNotes { get; set; }
    }
}
