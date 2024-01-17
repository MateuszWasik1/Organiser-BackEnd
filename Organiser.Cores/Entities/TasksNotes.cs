using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class TasksNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TNID { get; set; }
        public Guid TNGID { get; set; }
        public Guid TNTGID { get; set; }
        public int TNUID { get; set; }
        public string? TNNote { get; set; }
        public DateTime TNDate { get; set; }
        public DateTime? TNEditDate { get; set; }
    }
}