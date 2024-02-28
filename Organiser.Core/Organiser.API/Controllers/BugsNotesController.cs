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
        public List<BugsNotesViewModel> GetBugNotes(Guid bgid)
        {
            var bugNotes = new List<BugsNotes>();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int) RoleEnum.Support)
                bugNotes = context.AllBugsNotes.Where(x => x.BNBGID == bgid).OrderBy(x => x.BNDate).ToList();
            else
                bugNotes = context.BugsNotes.Where(x => x.BNBGID == bgid && !x.BNIsNewVerifier).OrderBy(x => x.BNDate).ToList();

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
                BNGID = Guid.NewGuid(),
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
            var isUserSupportOrAdmin = (currentUser?.URID == (int)RoleEnum.Admin || currentUser?.URID == (int)RoleEnum.Support);


            if (!isUserVerifier && isUserSupportOrAdmin)
            {

                if (string.IsNullOrEmpty(bug?.BAUIDS))
                    bug.BAUIDS = currentUser?.UGID.ToString();
                else
                    bug.BAUIDS = string.Join(",", bug.BAUIDS, currentUser?.UGID);

                context.CreateOrUpdate(bug);

                var bugNoteIsVerifier = new BugsNotes()
                {
                    BNGID = Guid.NewGuid(),
                    BNBGID = model.BNBGID,
                    BNUID = user.UID,
                    BNDate = DateTime.Now,
                    BNText = $"Nowym weryfikującym jest: {currentUser?.UFirstName} {currentUser?.ULastName} {currentUser?.UGID}",
                    BNIsNewVerifier = true,
                    BNIsStatusChange = false,
                };
                context.CreateOrUpdate(bugNoteIsVerifier);
            }

            context.CreateOrUpdate(bugNote);
            context.SaveChanges();
        }
    }
}
