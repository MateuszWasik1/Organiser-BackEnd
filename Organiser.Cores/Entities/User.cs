using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Organiser.Cores.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UID { get; set; }
        public Guid UGID { get; set; }
        public int URID { get; set; }
        public string? UFirstName { get; set; }
        public string? ULastName { get; set; }
        public string? UUserName { get; set; }
        public string? UEmail { get; set; }
        public string? UPhone { get; set; }
        public string? UPassword { get; set; }
    }
}