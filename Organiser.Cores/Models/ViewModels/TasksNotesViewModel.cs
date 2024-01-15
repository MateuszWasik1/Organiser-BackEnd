﻿namespace Organiser.Cores.Models.ViewModels
{
    public class TasksNotesViewModel
    {
        public int TNID { get; set; }
        public Guid TNGID { get; set; }
        public Guid TNTGID { get; set; }
        public int TNUID { get; set; }
        public int TNNote { get; set; }
        public DateTime TNDate { get; set; }
        public DateTime? TNEditDate { get; set; }
    }
}
