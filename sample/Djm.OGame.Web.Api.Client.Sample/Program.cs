using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Http;

namespace Djm.OGame.Web.Api.Client.Sample
{
    class Program
    {
        private static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cts.Cancel();
            };

            var task = Task.Run(() => RunAsync(cts.Token));

            task.Wait();

            Console.WriteLine("Press a key to quit");
            Console.ReadKey(true);
        }

        private static async Task RunAsync(CancellationToken cancellationToken)
        {
            IOGameClient client = new HttpOGameClient();
            try
            {
                /*var universes = await client.Universes.GetAllAsync(cancellationToken);
                foreach (var universe in universes.OrderBy(u => u.Id))
                {
                    Console.Write(universe.Id.ToString().PadLeft(3) + ".");
                    Console.WriteLine(universe.Name);
                }

                string input;
                int universeId;
                do
                {
                    Console.Write("Universe id? [10] ");
                    input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                        input = "10";

                    cancellationToken.ThrowIfCancellationRequested();
                }
                while (!int.TryParse(input, out universeId));*/


                var pin = await client.Universes[10].Pins.Add(104329, 106471, cancellationToken);
                Console.WriteLine(pin);
                
                //client.Universes[10].Pins.Delete(15, cancellationToken);
                //var alliance = await client.Universes[10].Alliances.GetDetailsAsync(500_000, cancellationToken);
//                var player = await client.Universes[10].Players.GetDetailsAsync(103973, cancellationToken);
//                Console.WriteLine(player.Name);
//                Console.WriteLine("Joueurs mis en favori : ");
//                foreach (var p in player.Favoris)
//                {
//                    Console.WriteLine(p.Name);
//                }
                // Console.WriteLine(alliance.Name);



                Console.WriteLine("Success");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Request was canceled ... :(");
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Exception: " + e.Message);
            }
            catch (WebException e)
            {
                Console.WriteLine("Web Exception: " + e.Message);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("HttpRequestException : " + e.Message);
            }
            catch (OgameNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (OgameBadRequestException e)
            {
                Console.WriteLine(e.Message);
            }


            Console.WriteLine("Ended successfully");
        }
    }
}
