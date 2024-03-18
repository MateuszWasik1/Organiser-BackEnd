namespace Organiser.Cores.Models.ViewModels.SavingsViewModels
{
    public class SavingViewModel
    {
        public Guid SGID { get; set; }
        public decimal SAmount { get; set; }
        public DateTime STime { get; set; }
        public string? SOnWhat { get; set; }
        public string? SWhere { get; set; }
    }
}
