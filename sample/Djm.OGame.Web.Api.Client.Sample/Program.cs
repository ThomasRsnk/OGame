using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Http;

namespace Djm.OGame.Web.Api.Client.Sample
{
    class Program
    {
        private static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            try
            {
                var task = Task.Run(() => RunAsync(cts.Token));

                task.Wait();
            }
            catch (AggregateException e)
            {
                e.Handle(x =>
                {
                    if (x is HttpRequestException)
                    {
                        Console.WriteLine(x.GetType() + " : " + x.Message);
                        return true;
                    }

                    if (x is Exception)
                    {
                        Console.WriteLine(x.GetType() + " : " + x.Message);
                        return true;
                    }
                    return true;

                });

            }
            
            
            Console.WriteLine("Press a key to quit");
            Console.ReadKey(true);
        }

        private static async Task RunAsync(CancellationToken cancellationToken)
        {
            IOGameClient client = new HttpOGameClient();
            var pin = await client.Universes[10].Pins.Add(104329, 106471, cancellationToken);
            Console.WriteLine(pin);
            return;
            


            Console.WriteLine("Ended successfully");
        }
    }
}
