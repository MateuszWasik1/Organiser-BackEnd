using Azure;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Dispatcher
{
    //public interface IDispatcher<TCommand>
    //{
    //    public void DispatchQuery();
    //    public void DispatchCommand();
    //}
    //public interface IDispatcher
    //{
    //    IQueryHandler<TQuery, TResult> DispatchQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    //}

    public interface IDispatcher
    {
        TResponse DispatchQuery<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
    }
}
