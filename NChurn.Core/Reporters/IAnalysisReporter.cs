using System.Collections.Generic;
using NChurn.Core.Analyzers;
using NChurn.Core.Processors;

namespace NChurn.Core.Reporters
{
    public interface IAnalysisReporter
    {
        void Write(AnalysisResult result, IProcessor<KeyValuePair<string,int>> cutoffPolicy, int topRecords);
    }
}