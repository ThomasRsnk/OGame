using System;
using System.Globalization;
using System.Net.Http;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class HttpUniverseResource : IUniverseResource
    {
        public HttpUniverseResource(int id)
        {
            var universeIdStr = id.ToString(CultureInfo.InvariantCulture);
            HttpClient = new HttpClientAdapter(new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:53388/api/universes/{universeIdStr}/")
            });

            Players = new PlayersHttpResource(HttpClient);
            Alliances = new AlliancesHttpResource(HttpClient);
            Scores = new ScoresHttpResource(HttpClient);
        }

        protected IHttpClient HttpClient { get; }
        
        public IPlayersResource Players { get; } 
        public IAlliancesResource Alliances { get; }
        public IScoresResource Scores { get; }
    }
}