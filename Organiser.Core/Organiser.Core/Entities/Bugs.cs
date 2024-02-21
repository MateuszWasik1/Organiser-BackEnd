using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class Bugs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BID { get; set; }
        public Guid BGID { get; set; }
        public int BUID { get; set; }
        public string? BAUIDS { get; set; }
        public DateTime CDate { get; set; }
        public string? CTitle { get; set; }
        public string? CText { get; set; }
    }
}