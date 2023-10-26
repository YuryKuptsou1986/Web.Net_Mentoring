namespace ConsoleWebApiClient.Clients
{
    internal interface IWebApiClient
    {
        Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(string path)
            where TEntity : class;
    }
}
