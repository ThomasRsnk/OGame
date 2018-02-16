using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client;
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
                var universes = await client.Universes.GetAllAsync(cancellationToken);
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
                while (!int.TryParse(input, out universeId));

                

                var alliances = await client.Universes[universeId].Alliances.GetAllAsync(cancellationToken);

                foreach (var alliance in alliances)
                    Console.WriteLine(alliance);

                Console.WriteLine("Success");
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Request was canceled ... :(");
            }


            Console.WriteLine("Ended successfully");
        }
    }
}
