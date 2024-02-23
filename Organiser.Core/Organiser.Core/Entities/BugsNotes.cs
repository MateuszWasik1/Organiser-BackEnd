using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class BugsNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BNID { get; set; }
        public Guid BNGID { get; set; }
        public int BNUID { get; set; }
        public DateTime BNDate { get; set; }
        public string? BNText { get; set; }
        public bool BNIsNewVerifier { get; set; }
        public bool BNIsStatusChange { get; set; }
    }
}