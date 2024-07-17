namespace Talabat.APIs.Specifications.ProductsSpecs
{
    public class ProductsSpecParams
    {
        private string? search ;
        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        private const int MaxPageSize = 10;
        private  int pageSize = 5;
        public int PageSize {
            get { return PageSize; }
            set { PageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; } 
        public int? BrandId { get; set; } 
        public int? CategoryId { get; set; } 
    }
}
