namespace Organiser.Cores.Models.ViewModels.CategoriesViewModel
{
    public class CategoriesViewModel
    {
        public int CID { get; set; }
        public Guid CGID { get; set; }
        public int CUID { get; set; }
        public string? CName { get; set; }
        public DateTime CStartDate { get; set; }
        public DateTime CEndDate { get; set; }
        public int? CBudget { get; set; }
        public decimal CBudgetCount { get; set; }

    }
}
