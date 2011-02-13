namespace NChurn.Core.Support
{
    public interface ICommandRunner
    {
        string ExecuteAndGetOutput(string command);
    }
}