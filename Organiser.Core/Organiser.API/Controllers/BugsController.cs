﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Models.Helpers;
using Organiser.Cores.Services;

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
        public List<BugsViewModel> GetBugs(BugTypeEnum bugType)
        {
            var bugs = new List<Bugs>();
            var currentUserRole = context.User.FirstOrDefault(x => x.UID == user.UID)?.URID ?? 1;

            if (currentUserRole == (int) RoleEnum.Admin || currentUserRole == (int)RoleEnum.Support)
            {
                if(bugType == BugTypeEnum.My)
                    bugs = context.AllBugs.Where(x => x.BUID == user.UID).OrderBy(x => x.BDate).ToList();

                else if (bugType == BugTypeEnum.ImVerificator)
                    bugs = context.AllBugs.Where(x => x.BAUIDS.Contains(user.UGID)).OrderBy(x => x.BDate).ToList();

                else if (bugType == BugTypeEnum.Closed)
                    bugs = context.AllBugs.Where(x => x.BStatus == BugStatusEnum.Rejected || x.BStatus == BugStatusEnum.Fixed).OrderBy(x => x.BDate).ToList();

                else
                    bugs = context.AllBugs.OrderBy(x => x.BDate).ToList();
            }

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
        public void ChangeBugStatus(ChangeBugStatusViewModel model)
        {
            var bug = context.Bugs.FirstOrDefault(x => x.BGID == model.BGID);

            if (bug == null)
                throw new Exception("Nie udało się zaaktualizować statusu błędu!");

            bug.BStatus = model.Status;

            var currentUser = context.User.FirstOrDefault(x => x.UID == user.UID);

            var bugNote = new BugsNotes()
            {
                BNGID = Guid.NewGuid(),
                BNBGID = bug.BGID,
                BNUID = user.UID,
                BNDate = DateTime.Now,
                BNText = $"Status został zmieniony na: \"{ChangeBugStatusToText.BugStatusText(model.Status)}\" przez użytkownika: {currentUser?.UFirstName} {currentUser?.ULastName}",
                BNIsNewVerifier = false,
                BNIsStatusChange = true,
                BNChangedStatus = model.Status,
            };

            context.CreateOrUpdate(bugNote);
            context.CreateOrUpdate(bug);
            context.SaveChanges();
        }
    }
}