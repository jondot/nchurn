using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NChurn.Core.Analyzers;
using NChurn.Core.Processors;

namespace NChurn.Core.Reporters
{
    public class TableReporter : IAnalysisReporter
    {
        private readonly TextWriter _out;
        public TableReporter(TextWriter outp)
        {
            _out = outp;
        }

        public void Write(AnalysisResult r, IProcessor<KeyValuePair<string,int>> cutoffPolicy , int top)
        {
            if (r.FileChurn.Any() == false)
                return;

            int max = r.FileChurn.Max(x => x.Key.Length);
            var i = r.FileChurn.FirstOrDefault().Value.ToString().Length;


            //padding
           
            var total = max + i + 3; //separators | .. | .. |
            string hline = "+".PadRight(total+3, '-')+"+";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(hline);
            foreach (var kvp in cutoffPolicy.Apply(r.FileChurn).Take(top))
            {
                sb.Append("| ").Append(kvp.Key.PadRight(max)).Append(" | ").Append(kvp.Value.ToString().PadRight(i)).AppendLine(" |");
            }
            sb.AppendLine(hline);
            _out.Write(sb.ToString());
            _out.Flush();
        }

    }
}