using ArquiteturaCamadas.Business.Settings.PaginationSettings;

namespace TestBuilders.Helpers
{
    public static class BuildersUtils
    {
        public static PageList<TEntity> BuildPageList<TEntity>(List<TEntity> entityList)
            where TEntity : class
            =>
            new PageList<TEntity>
            {
                CurrentPage = 1,
                PageSize = 10,
                Result = entityList,
                TotalCount = entityList.Count,
                TotalPages = 1
            };
    }
}
