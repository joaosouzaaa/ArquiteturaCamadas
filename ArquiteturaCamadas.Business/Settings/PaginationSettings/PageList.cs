namespace ArquiteturaCamadas.Business.Settings.PaginationSettings
{
    public sealed class PageList<TEntity>
        where TEntity : class
    {
        public List<TEntity> Result { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

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
