namespace Organiser.Cores.Models.ViewModels.CategoriesViewModel
{
    public class CategoryViewModel
    {
        public Guid CGID { get; set; }
        public string? CName { get; set; }
        public DateTime CStartDate { get; set; }
        public DateTime CEndDate { get; set; }
        public int? CBudget { get; set; }
        public decimal CBudgetCount { get; set; }
    }
}