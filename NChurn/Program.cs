using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using NChurn.Core.Analyzers;
using NChurn.Core.Reporters;
using NChurn.Core.Support;

namespace NChurn
{
    

    class Program
    {
        private static readonly HeadingInfo _headingInfo = new HeadingInfo("NChurn", ((AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0]).Version);

        private sealed class Options
        {
            [Option("d", "from-date",
                    Required = false,
                    HelpText = "Past date to calculate churn from, to now.")]
            public string Date = String.Empty;

            [Option("c", "churn", Required = false, HelpText = "Minimal churn rate. Churn results below are cut off.")]
            public int MinimalChurnRate = 0;

            [Option("t", "top", Required = false, HelpText = "Return this number of top records.")]
            public int Top = int.MaxValue;

            [Option("r", "report", Required = false, HelpText = "Type of report to output.")]
            public string Report = "table";


            [HelpOption(
                    HelpText = "Dispaly this help screen.")]

            public string GetUsage()
            {
                var help = new HelpText(Program._headingInfo);
                
                help.AdditionalNewLineAfterOption = true;
                help.AddPreOptionsLine(string.Format(@"Usage: {0}", Assembly.GetExecutingAssembly().GetName().Name));
                help.AddPreOptionsLine(string.Format(@"       {0} -c 4 -d 24-3-2010 -t 10", Assembly.GetExecutingAssembly().GetName().Name));
                help.AddOptions(this);

                return help;
            }
        }

        private static readonly Dictionary<string, Func<TextWriter, IAnalysisReporter>> _reporterMap =
            new Dictionary<string, Func<TextWriter, IAnalysisReporter>>();
        static Program()
        {
            _reporterMap.Add("table", x=> new TableReporter(x));
            _reporterMap.Add("csv", x=> new CSVReporter(x));
            _reporterMap.Add("xml", x=> new XMLReporter(x));

        }
        static void Main(string[] args)
        {
            var options = new Options();
            
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            if(!_reporterMap.ContainsKey(options.Report))
                options.Report = "table";


            try
            {
                Analyzer analyzer = Analyzer.Create();
                AnalysisResult analysisResult = string.IsNullOrEmpty(options.Date) ? analyzer.Analyze() : analyzer.Analyze(DateTime.ParseExact(options.Date,"dd-M-yyyy",CultureInfo.InvariantCulture));
                var tableReporter = _reporterMap[options.Report](Console.Out);
                tableReporter.Write(analysisResult, options.MinimalChurnRate, options.Top);
            }
            catch (CommandRunnerException e)
            {
                Console.Write(e.Message);
            }
         
        }
    }
}








