using NChurn.Core.Adapters.TF;
using NUnit.Framework;

namespace NChurn.Core.Tests.Integration.Adapters
{
    [TestFixture]
    class TfTest
    {
        [Test]
        public void test_tf_log()
        {
            TestHelpers.AssertAdapterFixture(new TFAdapter(), "fixtures/tf.log", "fixtures/tf.log.result");
        }
    }
}
