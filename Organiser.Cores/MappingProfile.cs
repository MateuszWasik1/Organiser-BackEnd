using AutoMapper;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Models.ViewModels.UserViewModels;

namespace Organiser.Cores
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categories, CategoriesViewModel>();
            CreateMap<Tasks, TasksViewModel>();
            CreateMap<TasksNotes, TasksNotesViewModel>();
            CreateMap<Savings, SavingsViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<User, UserAdminViewModel>();
        }
    }
}