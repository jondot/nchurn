using System.Diagnostics;
using System.IO;

namespace NChurn.Core.Support.Win32
{
    internal class Win32CommandRunner : ICommandRunner
    {
        public string ExecuteAndGetOutput(string command)
        {
            var processStartInfo = new ProcessStartInfo
                                       {
                                           Arguments = @"/c " + command,
                                           CreateNoWindow = true,
                                           FileName = "cmd",
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true,
                                           UseShellExecute = false

                                       };

            Process process = Process.Start(processStartInfo);
            string text;
            using (StreamReader standardOutput = process.StandardOutput)
            {
                text = standardOutput.ReadToEnd();
            }
            if (process.ExitCode != 0)
                throw new CommandRunnerException(string.Format("Failed executing: {0}", command));
          
            return text;            
        }
    }
}