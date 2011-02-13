using System.IO;
using Moq;
using NChurn.Core.Adapters;
using NChurn.Core.Analyzers;
using NChurn.Core.Reporters;
using NChurn.Core.Support;
using Shouldly;

namespace NChurn.Core.Tests
{
    public class TestHelpers
    {
        public static void AssertAdapterFixture(IVersioningAdapter gitAdapter, string fixturesGitLog, string fixturesGitLogResult)
        {
            var text = File.ReadAllText(fixturesGitLog);

            var mkresolver = new Mock<IAdapterResolver>();
            var mkrunner = new Mock<ICommandRunner>();
            mkrunner.Setup(x => x.ExecuteAndGetOutput(It.IsAny<string>())).Returns(text);

            gitAdapter.CommandRunner = mkrunner.Object;
            mkresolver.Setup(x => x.CreateAdapter()).Returns(gitAdapter);
            var analysisResult = new Analyzer(mkresolver.Object).Analyze();
            var s = new StringWriter();
           
            var tableReporter = new TableReporter(s);
            tableReporter.Write(analysisResult, 0, int.MaxValue);
            
            s.ToString().ShouldBe(File.ReadAllText(fixturesGitLogResult));
        }
    }
}
