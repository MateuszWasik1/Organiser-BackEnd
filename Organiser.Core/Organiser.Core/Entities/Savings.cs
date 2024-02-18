using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class Savings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SID { get; set; }
        public Guid SGID { get; set; }
        public int SUID { get; set; }
        public decimal SAmount { get; set; }
        public DateTime STime { get; set; }
        public string? SOnWhat { get; set; }
        public string? SWhere { get; set; }
    }
}