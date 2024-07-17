namespace Talabat.APIs.Helper
{
    public class Pagination<T>
    {
        public Pagination(int pageSize, int pageIndex, int count, IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
