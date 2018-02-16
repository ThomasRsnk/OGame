using System;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Http;
using Djm.OGame.Web.Api.Client.Http.Resources;
using Moq;
using NUnit.Framework;

namespace Djm.OGame.Web.Api.Client.Tests.Http.Resources
{
    [TestFixture]
    public class PlayersHttpResourceTests
    {

        private const string playersJson = @"[
            {
                ""id"": 1,
                ""name"": ""Legor""
            },
            {
                ""id"": 100172,
                ""name"": ""AO Gemini power""
            },
            {
                ""id"": 100470,
                ""name"": ""zhuguatchu""
            },
            {
                ""id"": 100557,
                ""name"": ""Lauviah""
            },
            {
                ""id"": 100796,
                ""name"": ""Raspoutine""
            }
        ]";

        private const string playerDetailsJson = @"{
        ""planets"": [
            {
                ""id"": 1,
                ""name"": ""Arakis"",
                ""coords"": ""1:1:2"",
                ""moon"": {
                    ""id"": 2,
                    ""name"": ""Lune"",
                    ""size"": 4998
                }
            }
        ],
        ""positions"": null,
        ""alliance"": null,
        ""status"": ""Administrator"",
        ""id"": 1,
        ""name"": ""Legor""
        }";

        public Mock<IHttpClient> HttpClientMock { get; set; }
        public PlayersHttpResource Players { get; set; }



        [SetUp]
        public void SetUp()
        {
            
            HttpClientMock = new Mock<IHttpClient>();

            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s== "players/1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(playerDetailsJson)
                });

            HttpClientMock
                .Setup(c => c.GetAsync(It.Is<string>(s => s == "players/"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(playersJson)
                });



            Players = new PlayersHttpResource(HttpClientMock.Object);
        }

        
        [Test]
        public async Task GetDetailsAsync_ShouldCalltheCorrectUrl()
        {
            await Players.GetDetailsAsync(1,default(CancellationToken));
            
            HttpClientMock.Verify(c => c.GetAsync("players/1", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldCalltheCorrectUrl()
        {
            await Players.GetAllAsync(default(CancellationToken));

            HttpClientMock.Verify(c => c.GetAsync("players/", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldParseJson()
        {
            var players = await Players.GetAllAsync(default(CancellationToken));
        
            Assert.That(players, Is.Not.Null);
            Assert.That(players, Has.Count.EqualTo(5));


        }

        [Test]
        public async Task GetDetailsAsync_ShouldParseJson()
        {
            var player = await Players.GetDetailsAsync(1, default(CancellationToken));
            Assert.That(player, Is.Not.Null);
        }
    }
}