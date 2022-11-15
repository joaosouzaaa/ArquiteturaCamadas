namespace ArquiteturaCamadas.Business.Settings.PaginationSettings
{
    public sealed class PageList<TEntity>
        where TEntity : class
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public List<TEntity> Result { get; private set; }
        
        public PageList() { }

        public PageList(List<TEntity> items, int count, PageParams pageParams)
        {
            Result = items;
            TotalCount = count;
            PageSize = pageParams.PageSize;
            CurrentPage = pageParams.PageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageParams.PageSize);
        }
    }
}
