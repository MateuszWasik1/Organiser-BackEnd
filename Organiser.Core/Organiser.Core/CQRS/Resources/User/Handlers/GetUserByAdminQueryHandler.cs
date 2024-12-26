using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Core.Exceptions;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.User.Handlers
{
    public class GetUserByAdminQueryHandler : IQueryHandler<GetUserByAdminQuery, UserAdminViewModel>
    {

        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetUserByAdminQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public UserAdminViewModel Handle(GetUserByAdminQuery query)
        {
            var userData = context.AllUsers.AsNoTracking().FirstOrDefault(x => x.UGID == query.UGID);

            if (userData == null)
                throw new UserNotFoundExceptions("Nie znaleziono użytkownika!");

            var model = mapper.Map<Cores.Entities.User, UserAdminViewModel>(userData);

            model.UCategoriesCount = context.AllCategories.Where(x => x.CUID == userData.UID).Count();
            model.UTasksCount = context.AllTasks.Where(x => x.TUID == userData.UID).Count();
            model.UTaskNotesCount = context.AllTasksNotes.Where(x => x.TNUID == userData.UID).Count();
            model.USavingsCount = context.AllSavings.Where(x => x.SUID == userData.UID).Count();

            return model;
        }
    }
}
