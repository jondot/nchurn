using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NChurn.Core.Support.Win32
{
    internal class Win32CommandRunnerDataSource : IAdapterDataSource
    {
        /// <summary>
        /// Context key to specify path includes for executables.
        /// </summary>
        public const string CT_EXE_PATH = "exe-path";

        private Dictionary<DataSourceContextKeys, object> _context = new Dictionary<DataSourceContextKeys, object>();

        public string GetDataWithQuery(string command)
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

            if (_context.ContainsKey(DataSourceContextKeys.ExePath))
            {
                processStartInfo.EnvironmentVariables["PATH"] = string.Format(
                    "{0};{1}",
                    processStartInfo.EnvironmentVariables["PATH"],
                    _context[DataSourceContextKeys.ExePath].ToString());
            }

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

        public void SetContext(DataSourceContextKeys varname, object value)
        {
            _context[varname] = value;
        }
    }
}