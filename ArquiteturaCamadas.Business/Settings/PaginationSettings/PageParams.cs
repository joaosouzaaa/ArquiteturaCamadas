namespace ArquiteturaCamadas.Business.Settings.PaginationSettings
{
    public sealed class PageParams
    {
        private const int MaxPageSize = 12;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value <= 0) ? _pageNumber : value;
        }
    }
}
