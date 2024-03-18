namespace Organiser.Cores.Models.ViewModels.SavingsViewModels
{
    public class SavingsViewModel
    {
        public int SID { get; set; }
        public Guid SGID { get; set; }
        public int SUID { get; set; }
        public decimal SAmount { get; set; }
        public DateTime STime { get; set; }
        public string? SOnWhat { get; set; }
        public string? SWhere { get; set; }
    }
}
