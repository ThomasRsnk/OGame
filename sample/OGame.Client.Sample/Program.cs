using System;
using System.Globalization;
using System.IO;
using System.Linq;
using OGame.Client.Models;

namespace OGame.Client.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var universe = new OgClient().Universe(10);

            foreach (var player in universe.GetPlayers())
            {
                Console.WriteLine(player);

                Console.Write("\t");
                //Console.WriteLine("Score : "+player.Score.Points);

                var alliance = player.Alliance;
                if (alliance == null) continue;

                Console.Write("\t");
                Console.WriteLine("Alliance : ");
                Console.Write("\t\t");
                Console.WriteLine("Name : " + alliance.Name);
                Console.Write("\t\t");
                Console.WriteLine("Founder : " + alliance.Founder.Name);
                Console.Write("\t\t");
                Console.WriteLine("Date : "+ alliance.FoundDate.ToString("yyyy/MM/dd",CultureInfo.InvariantCulture));
                Console.Write("\t\t");
                Console.WriteLine("Membres : ");
                foreach (var p in alliance.Members)
                {
                    Console.Write("\t\t\t");
                    Console.WriteLine(p.Name);
                }





            }

//            var p = universe.GetPlayerDetails(100172);
//            
//            Console.WriteLine(p);
//            Console.WriteLine(p.Alliance.Founder.Name);
            /*Console.WriteLine("Planets :");
            foreach (var planet in p.Planets)
            {
                Console.WriteLine(planet);
            }*/


            //Console.WriteLine(universe.GetPlanets(100796)[0].Name);


            /*foreach (var alli in universe.GetAlliances())
            {
                Console.WriteLine(alli.Name);
                foreach (var p in alli.Members)
                {
                    Console.Write("\t\t\t");
                    Console.WriteLine(p.Name);
                }
            }*/

            /*foreach (var score in universe.GetScores())
            {
                Console.WriteLine(score);
                Console.WriteLine("Name : "+score.Player.Name);
            }*/



            Console.ReadKey(true);
        }

        
    }
}
