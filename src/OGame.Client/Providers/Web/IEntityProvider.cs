namespace OGame.Client.Providers.Web
{
    internal interface IEntityProvider<in TKey, out TEntity>
    {
        TEntity Get(TKey id);
    }
}