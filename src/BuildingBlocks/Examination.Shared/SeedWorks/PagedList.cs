namespace Examination.Shared.SeedWorks
{
    public class PagedList<T> : PagedListBase
    {
        public List<T> Items { set; get; }

        public PagedList() { }
        public PagedList(List<T> items, long count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            Items = items;
        }
    }
}
