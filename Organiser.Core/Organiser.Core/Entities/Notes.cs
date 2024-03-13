using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class Notes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NID { get; set; }
        public Guid NGID { get; set; }
        public int NUID { get; set; }
        public DateTime NDate { get; set; }
        public DateTime NModificationDate { get; set; }
        public string? NTitle { get; set; }
        public string? NTxt { get; set; }
    }
}