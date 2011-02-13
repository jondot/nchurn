using System;

namespace NChurn.Core.Support
{
    public class CommandRunnerException : Exception
    {
        public CommandRunnerException(string format) : base(format)
        {
        }
    }
}