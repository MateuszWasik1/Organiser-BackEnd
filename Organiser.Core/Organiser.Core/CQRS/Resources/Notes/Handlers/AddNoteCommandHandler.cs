using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Cores.Context;
using Organiser.Cores.Services;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Handlers
{
    public class AddNoteCommandHandler : ICommandHandler<AddNoteCommand>
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        public AddNoteCommandHandler(IDataBaseContext context, IUserContext user) 
        {
            this.context = context;
            this.user = user;
        }

        public void Handle(AddNoteCommand command)
        {
            var note = new Cores.Entities.Notes()
            {
                NUID = user.UID,
                NGID = Guid.NewGuid(),
                NDate = DateTime.Now,
                NModificationDate = DateTime.Now,
                NTitle = command.Model.NTitle,
                NTxt = command.Model.NTxt,
            };

            context.CreateOrUpdate(note);
            context.SaveChanges();
        }
    }
}
