using NChurn.Core.Adapters.Git;
using NChurn.Core.Adapters.Hg;
using NUnit.Framework;

namespace NChurn.Core.Tests.Integration.Adapters
{
    [TestFixture]
    class HgTest
    {
        [Test]
        public void test_hg_log()
        {
            TestHelpers.AssertAdapterFixture(new HgAdapter(), "fixtures/hg.log", "fixtures/hg.log.result");
        }
    }
}
