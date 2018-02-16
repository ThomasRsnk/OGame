using System.Collections.Generic;
using System.Linq;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Cache
{
    internal class ProviderCache<TKey, TEntity> : IEntityProvider<TKey, TEntity>
    {
        public IEntityProvider<TKey, TEntity> SubProvider { get; }

        public Dictionary<TKey, TEntity> Cache { get; set; }

        public ProviderCache(IEntityProvider<TKey, TEntity> subProvider)
        {
            SubProvider = subProvider;
            Cache = new Dictionary<TKey, TEntity>();
        }
        
        public void Populate(TKey key, TEntity entity)
        {
            if (Cache.ContainsKey(key))
                Cache[key] = entity;
            else
                Cache.Add(key, entity);
        }

        public TEntity Get(TKey id)
        {
            if (!Cache.TryGetValue(id, out var entity))
            {
                Cache[id] = entity = SubProvider.Get(id);
            }

            return entity;
        }

        public List<TEntity> GetAll()
        {
            return Cache.Values.ToList();
        }
    }
}