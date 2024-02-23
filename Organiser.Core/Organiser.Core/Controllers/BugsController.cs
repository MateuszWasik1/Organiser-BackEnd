using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using System;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BugsController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public BugsController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetBugs")]
        public List<BugsViewModel> GetBugs()
        {
            var bugs = new List<Bugs>();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int) RoleEnum.Support)
                bugs = context.AllBugs.OrderBy(x => x.BDate).ToList();

            else
                bugs = context.Bugs.ToList();


            var bugsViewModel = new List<BugsViewModel>();

            bugs.ForEach(x =>
            {
                var bVM = mapper.Map<Bugs, BugsViewModel>(x);

                bugsViewModel.Add(bVM);
            });

            return bugsViewModel;
        }

        [HttpGet]
        [Route("GetBug")]
        public BugViewModel GetBug(Guid bgid)
        {
            var bug = new Bugs();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int)RoleEnum.Admin || currentUserRole == (int)RoleEnum.Support)
                bug = context.AllBugs.FirstOrDefault(x => x.BGID == bgid);
            else
                bug = context.Bugs.FirstOrDefault(x => x.BGID == bgid);

            if (bug == null)
                throw new Exception("Nie znaleziono wskazanego błędu !");

            var bugViewModel = mapper.Map<Bugs, BugViewModel>(bug);

            return bugViewModel;
        }

        [HttpPost]
        [Route("SaveBug")]
        public void SaveBug(BugViewModel model)
        {
            var bug = new Bugs()
            {
                BGID = model.BGID,
                BUID = user.UID,
                BDate = DateTime.Now,
                BTitle = model.BTitle,
                BText = model.BText,
                BStatus = model.BStatus,
            };

            context.CreateOrUpdate(bug);
            context.SaveChanges();
        }

        [HttpPost]
        [Route("ChangeBugStatus")]
        public void ChangeBugStatus(Guid bgid, BugStatusEnum status)
        {
            var bug = context.Bugs.FirstOrDefault(x => x.BGID == bgid);

            if (bug == null)
                throw new Exception("Nie udało się zaaktualizować statusu błędu!");

            bug.BStatus = status;

            context.CreateOrUpdate(bug);
            context.SaveChanges();
        }
    }
}
