using Djm.OGame.Web.Api.Controllers;
using Moq;
using NUnit.Framework;

namespace Djm.OGame.Web.Api.Dal.Tests
{
    [TestFixture]
    public class PinsControllerTests
    {
        public Mock<IOgameDb> OgameDbMock { get; set; }
        public PinsController Pins { get; set; }
    }
}