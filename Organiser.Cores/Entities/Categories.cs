namespace Organiser.Cores.Entities
{
    public class Categories
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