using NChurn.Core.Adapters.Svn;
using NUnit.Framework;

namespace NChurn.Core.Tests.Integration.Adapters
{
    [TestFixture]
    class SvnTest
    {
        [Test]
        public void test_svn_log()
        {
            TestHelpers.AssertAdapterFixture(new SvnAdapter(), "fixtures/svn.log", "fixtures/svn.log.result");
        }
    }
}
