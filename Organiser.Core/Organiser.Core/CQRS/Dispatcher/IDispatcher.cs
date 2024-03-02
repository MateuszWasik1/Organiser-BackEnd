using Organiser.CQRS.Abstraction.Commands;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Dispatcher
{
    public interface IDispatcher
    {
        TResponse DispatchQuery<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
        void DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
