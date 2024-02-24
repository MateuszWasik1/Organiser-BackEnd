using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BugsNotesController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public BugsNotesController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetBugNotes")]
        public List<BugsNotesViewModel> GetBugNotes()
        {
            var bugNotes = new List<BugsNotes>();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int) RoleEnum.Support)
                bugNotes = context.BugsNotes.OrderBy(x => x.BNDate).ToList();
            else
                bugNotes = context.BugsNotes.Where(x => !x.BNIsNewVerifier).OrderBy(x => x.BNDate).ToList();


            var bugNotesViewModel = new List<BugsNotesViewModel>();

            bugNotes.ForEach(x =>
            {
                var bNVM = mapper.Map<BugsNotes, BugsNotesViewModel>(x);

                bugNotesViewModel.Add(bNVM);
            });

            return bugNotesViewModel;
        }

        [HttpPost]
        [Route("SaveBugNote")]
        public void SaveBugNote(BugsNotesViewModel model)
        {
            var bugNote = new BugsNotes()
            {
                BNGID = model.BNGID,
                BNBGID = model.BNBGID,
                BNUID = user.UID,
                BNDate = DateTime.Now,
                BNText = model.BNText,
                BNIsNewVerifier = false,
                BNIsStatusChange = false,
            };

            var currentUser = context.User.FirstOrDefault(x => x.UID == user.UID);

            if (currentUser == null)
                throw new Exception("Nie znaleziono użytkownika");

            var bug = context.Bugs.FirstOrDefault(x => x.BGID == bugNote.BNBGID);

            if (bug == null)
                throw new Exception("Nie znaleziono wskazanego błędu!");

            var isUserVerifier = bug?.BAUIDS?.Contains(currentUser.UGID.ToString()) ?? false;

            if (!isUserVerifier)
            {
                bugNote.BNIsNewVerifier = true;

                //bug.BAUIDS = String.Join(",", bug.BAUIDS, currentUser.UGID)
                var strrring = String.Join(",", bug.BAUIDS, currentUser.UGID); //sprawdzić
                var xd = strrring;

                //context.CreateOrUpdate(bug);
            }

            context.CreateOrUpdate(bugNote);
            context.SaveChanges();
        }
    }
}
