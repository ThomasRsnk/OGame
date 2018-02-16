using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Http;
using Djm.OGame.Web.Api.Client.Http.Resources;
using Moq;
using NUnit.Framework;

namespace Djm.OGame.Web.Api.Client.Tests.Http.Resources
{
    [TestFixture]
    public class AllianceHttpResourceTests
    {
        private const string alliancesJson = @"[
        {
            ""id"": 500000,
            ""name"": ""Dragons Supremes""
        },
        {
            ""id"": 500001,
            ""name"": ""cereal serial killer""
        }]";

        private const string allianceDetailsJson = @"{
        ""id"": 500000,
        ""name"": ""Dragons Supremes"",
        ""tag"": ""D.S."",
        ""foundDate"": ""2005-03-29T15:53:52Z"",
        ""founder"": {
            ""id"": 119031,
            ""name"": ""Scoffield""
        },
        ""logo"": ""http://i32.servimg.com/u/f32/11/02/91/92/dragon10.jpg"",
        ""homePage"": ""http://dragonsupreme.ogameteam.net"",
        ""score"": {
            ""points"": 7452586,
            ""rank"": 288
        },
        ""members"": [
            {
                ""id"": 106262,
                ""name"": ""Bullshit""
            },
            {
                ""id"": 113872,
                ""name"": ""Gouverneur jb AO""
            },
            {
                ""id"": 119031,
                ""name"": ""Scoffield""
            }
        ]
    }";

        public Mock<IHttpClient> HttpClientMock { get; set; }
        public AlliancesHttpResource Alliances { get; set; }

        private const int BadId = -100;
        private const int CorrectId = 500000;

        [SetUp]
        public void SetUp()
        {
            HttpClientMock = new Mock<IHttpClient>();

            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s=="alliances/"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(alliancesJson)
                });

            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s == "alliances/"+CorrectId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(allianceDetailsJson)
                });

            Alliances = new AlliancesHttpResource(HttpClientMock.Object);
        }

        [Test]
        public async Task GetAllAsynk_ShouldParseJson()
        {
            var alliances = await Alliances.GetAllAsync(default(CancellationToken));

            Assert.That(alliances, Is.Not.Null);
            Assert.That(alliances, Has.Count.EqualTo(2));

            Assert.That(alliances[0],Has.Property("Id").EqualTo(500000));
            Assert.That(alliances[0], Has.Property("Name").EqualTo("Dragons Supremes"));
            Assert.That(alliances[1], Has.Property("Id").EqualTo(500001));
            Assert.That(alliances[1], Has.Property("Name").EqualTo("cereal serial killer"));



        }

        [Test]
        public async Task GetDetailsAsync_ShouldParseJson()
        {
            var alliance = await Alliances.GetDetailsAsync(CorrectId, default(CancellationToken));

            Assert.That(alliance,Is.Not.Null);
        }


        [Test]
        public void GetDetailsAsync_ShouldThrowNotFoundException()
        {
            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s == "alliances/"+BadId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.NotFound));

            Assert.Throws<OgameNotFoundException>(() => Alliances.GetDetailsAsync(BadId, default(CancellationToken)));
        }   

        












    
    }
}