using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavingsController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IUserContext user;
        private readonly IMapper mapper;
        public SavingsController(IDataBaseContext context, IUserContext user, IMapper mapper)
        {
            this.context = context;
            this.user = user;
            this.mapper = mapper;
        }

        [HttpGet]
        public List<SavingsViewModel> Get()
        {
            var savings = context.Savings.OrderBy(x => x.STime).ToList();

            var savingsViewModel = new List<SavingsViewModel>();

            savings.ForEach(x =>
            {
                var sVM = mapper.Map<Savings, SavingsViewModel>(x);
                savingsViewModel.Add(sVM);
            });

            return savingsViewModel;
        }

        [HttpPost]
        [Route("Save")]
        public void Save(SavingsViewModel model)
        {
            if (model.SID == 0)
            {
                var saving = new Savings()
                {
                    SGID = model.SGID,
                    SUID = user.UID,
                    SAmount = model.SAmount,
                    STime = model.STime,
                    SOnWhat = model.SOnWhat,
                    SWhere = model.SWhere,
                };

                context.CreateOrUpdate(saving);
            }
            else
            {
                var saving = context.Savings.FirstOrDefault(x => x.SGID == model.SGID);

                if (saving == null)
                    throw new Exception("Nie znaleziono oszczędności");

                saving.SAmount = model.SAmount;
                saving.STime = model.STime;
                saving.SOnWhat = model.SOnWhat;
                saving.SWhere = model.SWhere;
            }

            context.SaveChanges();
        }

        [HttpDelete]
        [Route("Delete/{sGID}")]
        public void Delete(Guid sGID)
        {
            var saving = context.Savings.FirstOrDefault(x => sGID == x.SGID);

            if (saving == null)
                throw new Exception("Nie znaleziono oszczędności");

            context.DeleteSaving(saving);
            context.SaveChanges();
        }
    }
}