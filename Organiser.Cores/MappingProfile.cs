using AutoMapper;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.Cores
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Categories, CategoriesViewModel>();
        }
    }
}
