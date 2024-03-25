using Organiser.Cores.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organiser.Cores.Entities
{
    public class TasksSubTasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TSTID { get; set; }
        public Guid TSTGID { get; set; }
        public Guid TSTTGID { get; set; }
        public int TSTUID { get; set; }
        public string? TSTTitle { get; set; }
        public string? TSTNext { get; set; }
        public DateTime TSTCreationDate { get; set; }
        public DateTime? TSTModifyDate { get; set; }
        public SubTasksStatusEnum TSTStatus { get; set; }
    }
}