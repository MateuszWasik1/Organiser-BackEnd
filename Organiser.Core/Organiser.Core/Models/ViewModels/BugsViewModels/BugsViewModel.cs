﻿using Organiser.Cores.Models.Enums;

namespace Organiser.Core.Models.ViewModels.BugsViewModels
{
    public class BugsViewModel
    {
        public int BID { get; set; }
        public Guid BGID { get; set; }
        public int BUID { get; set; }
        public string? BVerifiers { get; set; }
        public DateTime BDate { get; set; }
        public string? BTitle { get; set; }
        public string? BText { get; set; }
        public BugStatusEnum? BStatus { get; set; }
    }
}