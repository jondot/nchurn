using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;
using NChurn.Core.Adapters;
using NChurn.Core.Adapters.Git;
using NChurn.Core.Adapters.Hg;
using NChurn.Core.Adapters.Svn;
using NChurn.Core.Adapters.TF;
using NChurn.Core.Analyzers;
using NChurn.Core.Processors;
using NChurn.Core.Processors.Cutoff;
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
                    HelpText = "Past date to calculate churn from. Absolute in dd-mm-yyyy or number of days back from now.")]
            public string Date = String.Empty;

            [Option("c", "churn", Required = false, HelpText = "Minimal churn. Specify either a number for minimum, or float for precent.")]
            public string MinimalChurnRate = "0";

            [Option("t", "top", Required = false, HelpText = "Return this number of top records.")]
            public int Top = int.MaxValue;

            [Option("r", "report", Required = false, HelpText = "Type of report to output. Use one of: table (default), xml, csv")]
            public string Report = "table";

            [Option("a", "adapter", Required = false, HelpText = "Use a specific versioning adapter. Use one of: auto (default), git, tf, svn, hg")]
            public string Adapter = "auto";

            [Option("p", "env-path", Required = false, HelpText = @"Add to PATH. i.e. for svn.exe you might add ""c:\tools"". Can add multiple with ;.")]
            public string EnvPath = null;

            [Option("i", "input", Required = false, HelpText = "Get input from a file instead of running a versioning system. Must specify correct adapter via -a.")]
            public string InputFile = null;

            [Option("x", "exclude", Required = false, HelpText = "Exclude resources matching this regular expression")]
            public string ExcludePattern = null;

            [Option("n", "include", Required = false, HelpText = "Include resources matching this regular expression")]
            public string IncludePattern = null;


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

        private static readonly Dictionary<string, Func<string , IVersioningAdapter>> _adapterMap =
            new Dictionary<string, Func<string, IVersioningAdapter>>();

        static Program()
        {
            _reporterMap.Add("table", x=> new TableReporter(x));
            _reporterMap.Add("csv", x=> new CSVReporter(x));
            _reporterMap.Add("xml", x=> new XMLReporter(x));

            _adapterMap.Add("git", x => WithPath(x, new GitAdapter()));
            _adapterMap.Add("svn", x => WithPath(x,new SvnAdapter()));
            _adapterMap.Add("tf", x => WithPath(x,new TFAdapter()));
            _adapterMap.Add("hg", x => WithPath(x,new HgAdapter()));
            _adapterMap.Add("auto", x => WithPath(x, new AutoDiscoveryAdapter()));
        }

        private static IVersioningAdapter WithPath(string x, IVersioningAdapter gitAdapter)
        {
            if(!string.IsNullOrEmpty(x))
            {
                gitAdapter.DataSource.SetContext(DataSourceContextKeys.ExePath, x);
            }
            return gitAdapter;
        }

        static void Main(string[] args)
        {
            var options = new Options();
            
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            if(!_reporterMap.ContainsKey(options.Report))
                options.Report = "table";
            
            if(!_adapterMap.ContainsKey(options.Adapter))
            {
                Console.Error.WriteLine(string.Format("Warning: {0} does not exist.", options.Adapter));
                options.Adapter = "auto";
            }

            try
            {
                //
                // set up cutoff
                //

                IProcessor<KeyValuePair<string, int>> cutoffPolicy = new MinimalCutoffProcessor(0);
                int minchurn;
                if(int.TryParse(options.MinimalChurnRate, out minchurn))
                {
                    cutoffPolicy = new MinimalCutoffProcessor(minchurn);
                }
                else
                {
                    float minchurnpcent;
                    if(float.TryParse(options.MinimalChurnRate, out minchurnpcent))
                    {
                        cutoffPolicy = new PrecentCutoffProcessor(minchurnpcent);
                    }
                }

                //
                // set up analyzer
                //
                Analyzer analyzer = Analyzer.Create(_adapterMap[options.Adapter](options.EnvPath));
                
                if (!string.IsNullOrEmpty(options.IncludePattern))
                    analyzer.AddInclude(options.IncludePattern);
                if(!string.IsNullOrEmpty(options.ExcludePattern))
                    analyzer.AddExclude(options.ExcludePattern);
                
                if(options.InputFile != null && !File.Exists(options.InputFile))
                {
                    Console.Error.WriteLine(string.Format("Cannot file file {0} to read from.", options.InputFile));
                    ExitWithError();
                }



                //
                // perform analysis
                //

                AnalysisResult analysisResult;
                if(options.InputFile != null)
                {
                    analysisResult = analyzer.Analyze(File.ReadAllText(options.InputFile));
                }
                else
                {
                    analysisResult = string.IsNullOrEmpty(options.Date) ? analyzer.Analyze() 
                                                                        : GetAnalysisResultWithDateBackAnalysis(options, analyzer);
                }
                
                Console.OutputEncoding = Encoding.UTF8;
                var reporter = _reporterMap[options.Report](Console.Out);
                reporter.Write(analysisResult, cutoffPolicy, options.Top);
            }
            catch (CommandRunnerException e)
            {
                Console.Error.WriteLine(string.Format("Error: {0}. You should make sure that your versioning tool is accessible via command line (set PATH).", e.Message));
                ExitWithError();
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(string.Format("Error: {0}.", e.Message));
                ExitWithError();
            }

        }

        private static AnalysisResult GetAnalysisResultWithDateBackAnalysis(Options options, Analyzer analyzer)
        {
            DateTime dt;
            if(!DateTime.TryParseExact(options.Date, "dd-M-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Now.Subtract(TimeSpan.FromDays(int.Parse(options.Date))); //crash hard if not really an int.
            }
            
            return analyzer.Analyze(dt);
        }

        private static void ExitWithError()
        {
            Environment.Exit(1);
        }
    }
}








