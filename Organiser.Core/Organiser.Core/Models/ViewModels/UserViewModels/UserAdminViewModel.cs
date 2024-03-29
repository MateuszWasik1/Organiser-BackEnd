﻿namespace Organiser.Cores.Models.ViewModels.UserViewModels
{
    public class UserAdminViewModel
    {
        public int UID { get; set; }
        public Guid UGID { get; set; }
        public int URID { get; set; }
        public string? UFirstName { get; set; }
        public string? ULastName { get; set; }
        public string? UUserName { get; set; }
        public string? UEmail { get; set; }
        public string? UPhone { get; set; }
        public int UCategoriesCount { get; set; }
        public int UTasksCount { get; set; }
        public int UTaskNotesCount { get; set; }
        public int USavingsCount { get; set; }
    }
}
