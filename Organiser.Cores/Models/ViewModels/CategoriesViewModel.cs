namespace Organiser.Cores.Models.ViewModels
{
    public class CategoriesViewModel
    {
        public int CID { get; set; }
        public Guid CGID { get; set; }
        public int CUID { get; set; }
        public string? CName { get; set; }
        public DateTime CStartDate { get; set; }
        public DateTime CEndDate { get; set; }
        public decimal CBudget { get; set; }
    }
}
