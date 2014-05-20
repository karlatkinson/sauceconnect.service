using NUnit.Framework;

namespace SauceConnect.Service.SlowTests
{
    [TestFixture]
    public class SauceLabsRestApiTests
    {
        [Test]
        public void can_get_list_of_tunnels()
        {
            var sauceLabsRestClient = new SauceLabsRestClient("karlatkinson", "ACCESSKEY");
            var existingTunnels = sauceLabsRestClient.GetExistingTunnels();

            Assert.That(existingTunnels.Count, Is.GreaterThan(0));
        }
    }
}
