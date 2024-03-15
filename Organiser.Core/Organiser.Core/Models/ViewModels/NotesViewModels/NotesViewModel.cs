namespace Organiser.Core.Models.ViewModels.NotesViewModels
{
    public class NotesViewModel
    {
        public Guid NGID { get; set; }
        public DateTime NDate { get; set; }
        public DateTime NModificationDate { get; set; }
        public string? NTitle { get; set; }
        public string? NTxt { get; set; }
    }
}