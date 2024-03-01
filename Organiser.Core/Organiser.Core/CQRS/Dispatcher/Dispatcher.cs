using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Dispatcher
{
    //public class Dispatcher : IDispatcher<in TQuery, TResponse>
    //{
    //    public void DispatchQuery()
    //    {

    //    }

    //    public void DispatchCommand()
    //    {

    //    }
    //}

    //public class Dispatcher
    //{
    //    public void DispatchQuery()
    //    {

    //    }

    //    public void DispatchCommand<TCommand>(TCommand command) where TCommand : class
    //    {
    //        Type handler = typeof(ICommandHandler<>);
    //        Type handlerType = handler.MakeGenericType(command.GetType());

    //        Type[] concreteTypes = Assembly.GetExecutingAssembly().GetTypes()
    //            .Where(t => t.IsClass && t.GetInterfaces().Contains(handlerType))
    //            .ToArray();

    //        if (!concreteTypes.Any())
    //            return;

    //        foreach(Type type in concreteTypes)
    //        {
    //            var concreteHandler = Activator.CreateInstance(type) as ICommandHandler<TCommand>;

    //            concreteHandler.Handle(command);
    //        }

    //    }
    //}

    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public TResponse DispatchQuery<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
        {
            var queryHandler = serviceProvider.GetService<IQueryHandler<TQuery, TResponse>>();

            return queryHandler.Handle(query);
        }
    }
}
