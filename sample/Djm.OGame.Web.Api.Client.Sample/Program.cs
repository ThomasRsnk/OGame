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
                        case OgameException ex:
                            Console.WriteLine(ex.GetType() + " : " + ex.Message);
                            return true;
                        case OperationCanceledException ex:
                            Console.WriteLine("La requête a été annulée");
                            return true;
                        case FileNotFoundException ex:
                            Console.WriteLine(ex.GetType() + " : " + ex.Message);
                            return true;
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

            var pin = await client.Universes[110].Pins.Add(104329, 106471, cancellationToken);
            //var universes = await client.Universes.GetAllAsync(cancellationToken);
            //await client.Universes[10].Pictures.Set(121597, "pic4.jpg", cancellationToken);



            Console.WriteLine(pin);

        }
    }
}
