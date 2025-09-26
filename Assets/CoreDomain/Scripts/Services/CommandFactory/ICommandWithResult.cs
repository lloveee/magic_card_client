namespace CoreDomain.Scripts.Services.CommandFactory
{
    public interface ICommandWithResult<out TResult>: IBaseCommand
    {
        TResult Execute();
    }
}