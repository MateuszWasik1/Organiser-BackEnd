namespace Organiser.Cores.Models.ViewModels
{
    public class TasksViewModel
    {
        public int TID { get; set; }
        public Guid TGID { get; set; }
        public int TUID { get; set; }
        public string? TName { get; set; }
        public string? TLocalization { get; set; }
        public DateTime TTime { get; set; }
        public int TBudget { get; set; }
    }
}
