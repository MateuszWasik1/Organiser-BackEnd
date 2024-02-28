namespace Organiser.CQRS.Abstraction.Commands
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        bool Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    {
        TResponse Handle(TCommand command);
    }
}

