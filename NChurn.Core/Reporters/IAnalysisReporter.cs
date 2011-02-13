using NChurn.Core.Analyzers;

namespace NChurn.Core.Reporters
{
    public interface IAnalysisReporter
    {
        void Write(AnalysisResult result, int minimalChurnRate, int topRecords);
    }
}