using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Models
{
    public class PaginationSearchResult
    {
        public int Page { get; set; } = 1;
        public int PageSize {  get; set; }
        public string SearchValue {  get; set; }
        public int RowCount {  get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int n = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    n += 1;
                }
                return n;
            }
        }
        
    }
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }

    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }

    public class ProductSearchResult : PaginationSearchResult
    {
        public required List<Product> Data { get; set; }
    }
    public class OrderSearchResult : PaginationSearchResult
    {
        
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }

}
