using NChurn.Core.Adapters.Git;
using NUnit.Framework;

namespace NChurn.Core.Tests.Integration.Adapters
{
    [TestFixture]
    class GitTest
    {
        [Test]
        public void test_git_log()
        {
            TestHelpers.AssertAdapterFixture(new GitAdapter(), "fixtures/git.log", "fixtures/git.log.result");
        }

    }
}
