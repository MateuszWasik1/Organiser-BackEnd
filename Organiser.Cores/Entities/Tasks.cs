using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class Tasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TID { get; set; }
        public Guid TGID { get; set; }
        public int TUID { get; set; }
        public string? TName { get; set; }
        public string? TLocalization { get; set; }
        public DateTime TTime { get; set; }
        public int TBudget { get; set; }
    }
}