using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Http;


namespace OGameTestFront
{
    internal class Program
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



                var alliance = await client.Universes[10].Alliances.GetDetailsAsync(500_000, cancellationToken);


                Console.WriteLine(alliance.Name);

                Console.WriteLine("Success");
            }
            catch (OperationCanceledException e)
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
                Console.WriteLine("HttpRequestException : "+ e.Message);
            }
            catch (OgameNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }


            Console.WriteLine("Ended successfully");
        }
    }
}
