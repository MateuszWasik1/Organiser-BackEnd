using AutoMapper;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.Cores
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categories, CategoriesViewModel>();
            CreateMap<Tasks, TasksViewModel>();
        }
    }
}