﻿using Organiser.Core.Exceptions.Categories;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Resources.Categories.Commands;

namespace Organiser.CQRS.Resources.Categories.Handlers
{
    public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddCategoryCommandHandler(IDataBaseContext context, IUserContext user)
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddCategoryCommand command)
        {
            if (command.Model.CName.Length == 0)
                throw new CategoryNameRequiredException("Nazwa kategorii nie może być pusta!");

            if (command.Model.CName.Length > 300)
                throw new CategoryNameMax300Exception("Nazwa kategorii nie może przekraczać 300 znaków!");

            if (command.Model.CBudget < 0)
                throw new CategoryBudgetMin0Exception("Budżet nie może być mniejszy od zera!");

            var category = new Cores.Entities.Categories()
            {
                CGID = Guid.NewGuid(),
                CUID = user.UID,
                CName = command.Model.CName,
                CStartDate = command.Model.CStartDate,
                CEndDate = command.Model.CEndDate,
                CBudget = command.Model.CBudget,
            };

            context.CreateOrUpdate(category);
            context.SaveChanges();
        }
    }
}
