using System.Collections.Generic;

namespace ProjectManager.Shared.Common.Pagging
{
    public interface IPagedResults<T>
    {
        IEnumerable<T> Data { get; set; }
        long TotalRecords { get; set; }
        string ResponseMessage { get; set; }
        int ResponseCode { get; set; }
    }

    public class PagedResults<T> : IPagedResults<T>
    {
        public PagedResults() { }
        public PagedResults(IEnumerable<T> data, int totalCount)
        {
            Data = data;
            TotalRecords = totalCount;
        }

        public long TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
