using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OGame.Client.Providers.Cache;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Log
{
    internal class LogProvider<TKey, TEntity> : IEntityProvider<TKey, TEntity>
    {
        public string Prefix { get; set; }
        public IEntityProvider<TKey, TEntity> SubProvider { get; }

        public LogProvider(string prefix, IEntityProvider<TKey, TEntity> subProvider)
        {
            Prefix = prefix;
            SubProvider = subProvider;
        }

        public TEntity Get(TKey id)
        {
            //Log($"Get({id})");
            return SubProvider.Get(id);
        }
        

        private void Log(string logMessage)
        {
            Debug.WriteLine($"{Prefix} : {logMessage}");
            using (var w = File.AppendText("log.txt"))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine("  :{0}", Prefix);
                w.WriteLine("  :{0}", logMessage);
                w.WriteLine("-------------------------------");
            }

        }
    }
}