using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Http;

namespace Djm.OGame.Web.Api.Client.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            try
            {
                var task = Task.Run(() => RunAsync(cts.Token));

                task.Wait(cts.Token);
            }
            catch (AggregateException e)
            {
                e.Handle(x =>
                {
                    switch (x)
                    {
                        case AggregateException ex:
                            Console.WriteLine(ex.InnerException.GetType() + " : " + ex.InnerException.Message);
                            return true;
                        default: 
                            Console.WriteLine(x.GetType() + " : " + x.Message);
                            return true;
                    }
                 });
            }

            Console.WriteLine("Press a key to quit");
            Console.ReadKey(true);
        }

        private static async Task RunAsync(CancellationToken cancellationToken)
        {
            IOGameClient client = new HttpOGameClient();

            //exemple d'utilisations : 

            //FAVORIS (PINS)
            //var pin = await client.Universes[10].Pins.Add(104329, 106471, cancellationToken);
            //Console.WriteLine(pin);

            //LISTE UNIVERS
            //var universes = await client.Universes.GetAllAsync(cancellationToken);

            //PROFILE PICTURE
            //            var uploadIsSuccessful = await client.Universes[100].Pictures.Set(121597, "pic4.jpg", cancellationToken);
            //            Console.WriteLine("upload :"+ uploadIsSuccessful);

            //SCORES
            //            var scores = await client.Universes[100].Scores.GetAllForPlayersAsync(3,cancellationToken);
            //
            //            foreach(var score in scores)
            //                Console.WriteLine(score.Player.Name +" : "+score.Points);

            //var scores = await client.Universes[100].Scores.GetAllForAlliancesAsync(cancellationToken);

            //LISTE ALLIANCES
            //            var alliances = await client.Universes[10].Alliances.GetAllAsync(cancellationToken);
            //            foreach (var alli in alliances)
            //            {
            //                Console.WriteLine(alli.Name);
            //            }

            //PLAYER DETAILS   
            //            var player = await client.Universes[10].Players.GetDetailsAsync(100172, cancellationToken);
            //
            //            Console.WriteLine(player.Name);
            //            Console.WriteLine(player.Alliance?.Name);
            //            foreach (var pos in player.Positions)
            //            {
            //                Console.WriteLine(pos.Type+" "+ pos.Rank+" "+pos.Score);
            //            }

            //CONNECTION
            //            var x = await client.Universes[10].Players.Connect("Caligula", cancellationToken);
            //            Console.WriteLine("Connection : "+x);
        }
    }
}
