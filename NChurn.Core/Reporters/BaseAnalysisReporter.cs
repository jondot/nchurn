using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NChurn.Core.Analyzers;

namespace NChurn.Core.Reporters
{
    public abstract class BaseAnalysisReporter : IAnalysisReporter
    {
        protected TextWriter _out;

        protected BaseAnalysisReporter(TextWriter outp)
        {
             _out = outp;
        }

        public void Write(AnalysisResult r, int minimalChurnRate, int topRecords)
        {
            if (r.FileChurn.Any() == false)
                return;

            IEnumerable<KeyValuePair<string, int>> fileChurns = r.FileChurn.Where(x => x.Value > minimalChurnRate).Take(topRecords);

            WriteImpl(fileChurns);
            _out.Flush();
        }

        protected abstract void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns);
    }
}
