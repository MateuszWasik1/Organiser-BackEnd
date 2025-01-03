﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Savings.Handlers
{
    public class GetSavingsQueryHandler : IQueryHandler<GetSavingsQuery, GetSavingsViewModel>
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetSavingsQueryHandler(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GetSavingsViewModel Handle(GetSavingsQuery query)
        {
            var savings = context.Savings.OrderBy(x => x.STime).AsNoTracking().ToList();

            var savingsViewModel = new List<SavingsViewModel>();

            var count = savings.Count;
            savings = savings.Skip(query.Skip).Take(query.Take).ToList();

            savings.ForEach(x =>
            {
                var sVM = mapper.Map<Cores.Entities.Savings, SavingsViewModel>(x);
                savingsViewModel.Add(sVM);
            });

            var model = new GetSavingsViewModel()
            {
                List = savingsViewModel,
                Count = count
            };

            return model;
        }
    }
}
