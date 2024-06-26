﻿using AutoMapper;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.Cores.Models.ViewModels.UserViewModels;

namespace Organiser.Cores
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categories, CategoryViewModel>();
            CreateMap<Categories, CategoriesViewModel>();
            CreateMap<Tasks, TaskViewModel>();
            CreateMap<Tasks, TasksViewModel>();
            CreateMap<TasksNotes, TasksNotesViewModel>();
            CreateMap<TasksNotes, TasksNotesAddViewModel>();
            CreateMap<TasksSubTasks, TasksSubTasksViewModel>();
            CreateMap<Savings, SavingsViewModel>();
            CreateMap<Savings, SavingViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<User, UsersAdminViewModel>();
            CreateMap<User, UserAdminViewModel>();
            CreateMap<Bugs, BugsViewModel>();
            CreateMap<Bugs, BugViewModel>();
            CreateMap<BugsNotes, BugsNotesViewModel>();
            CreateMap<Notes, NotesViewModel>();
        }
    }
}