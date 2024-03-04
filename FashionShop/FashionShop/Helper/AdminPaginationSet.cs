namespace FashionShop.Helper
{
    public class AdminPaginationSet<T>
    {
        public int Page { get; set; }
        public int Count
        {
            get
            {
                return (List != null) ? List.Count() : 0;
            }
        }

        // Lưu tổng số trang
        public int PagesCount { get; set; }

        // Lưu tổng số bản ghi
        public int TotalCount { get; set; }

        public IEnumerable<T> List { get; set; }
    }
}
