using Organiser.Cores.Models.Enums;
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
        public DateTime BDate { get; set; }
        public string? BTitle { get; set; }
        public string? BText { get; set; }
        public BugStatusEnum? BStatus { get; set; }
    }
}