namespace ProjectManager.Shared.Model.Request
{
    public class PagingRequest
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string SortField { get; set; }
        public string SortDir { get; set; }
        public string SearchText { get; set; }
    }
}
