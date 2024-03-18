using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Savings.Queries
{
    public class GetSavingsQuery : IQuery<List<SavingsViewModel>>
    {
    }
}
