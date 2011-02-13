using System.Collections.Generic;
using System.Linq;

namespace NChurn.Core.Analyzers
{
    public class AnalysisResult
    {
        public IOrderedEnumerable<KeyValuePair<string, int>> FileChurn { get; set; }
    }
}