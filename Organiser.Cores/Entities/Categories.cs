using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CID { get; set; }
        public Guid CGID { get; set; }
        public int CUID { get; set; }
        public string? CName { get; set; }
        public DateTime CStartDate { get; set; }
        public DateTime CEndDate { get; set; }
        public int? CBudget { get; set; }
    }
}