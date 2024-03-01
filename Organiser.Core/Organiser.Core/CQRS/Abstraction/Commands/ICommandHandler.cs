namespace Organiser.CQRS.Abstraction.Commands
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }

    public interface ICommandBoolHandler<TCommand> where TCommand : ICommand
    {
        bool Handle(TCommand command);
    }

    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        TResponse Handle(TCommand command);
    }
}

