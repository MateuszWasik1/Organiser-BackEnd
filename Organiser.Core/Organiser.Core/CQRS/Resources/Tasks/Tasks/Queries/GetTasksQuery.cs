﻿using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries
{
    public class GetTasksQuery : IQuery<List<TasksViewModel>>
    {
        public string? CGID { get; set; }
        public int Status { get; set; }
    }
}
