using Djm.OGame.Web.Api.Client.Http;
using NUnit.Framework;

namespace Djm.OGame.Web.Api.Client.Tests.Http
{
    [TestFixture]
    public class HttpOGameClientTests
    {
        protected HttpOGameClient Client { get; set; }

        [SetUp]
        public void SetUp()
        {
            Client = new HttpOGameClient();
        }

        [Test]
        public void Universes_should_be_not_null()
        {
            Assert.That(Client.Universes, Is.Not.Null);
        }
    }
}