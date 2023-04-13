namespace ProjectManager.Shared.Common.Pagging
{
    public interface IPagedResult<T>
    {
        T Data { get; set; }
        string ResponseMessage { get; set; }
        int ResponseCode { get; set; }
    }

    public class PagedResult<T> : IPagedResult<T>
    {
        public T Data { get; set; }
        public string ResponseMessage { get; set; }
        public int ResponseCode { get; set; }
    }
}
