using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NChurn.Core.Analyzers;
using NChurn.Core.Processors;

namespace NChurn.Core.Reporters
{
    public abstract class BaseAnalysisReporter : IAnalysisReporter
    {
        protected TextWriter _out;

        protected BaseAnalysisReporter(TextWriter outp)
        {
             _out = outp;
        }

        public void Write(AnalysisResult r, IProcessor<KeyValuePair<string,int>> cutoffPolicy, int topRecords)
        {
            if (r.FileChurn.Any() == false)
                return;

            IEnumerable<KeyValuePair<string, int>> fileChurns = cutoffPolicy.Apply(r.FileChurn).Take(topRecords);

            WriteImpl(fileChurns);
            _out.Flush();
        }
        protected abstract void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns);
    }
}
