using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NChurn.Core.Analyzers;

namespace NChurn.Core.Reporters
{
    public class CSVReporter : BaseAnalysisReporter
    {
        private const string _sep = ",";

        public CSVReporter(TextWriter outp) : base(outp)
        {
        }


        protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var kvp in fileChurns)
            {
                sb.Append("\"").Append(kvp.Key).Append("\"").Append(_sep).Append(kvp.Value);
            }
            _out.Write(sb.ToString());
        }
    }
}
