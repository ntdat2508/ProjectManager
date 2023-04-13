namespace ProjectManager.Authentication.Model
{
    public class Response<T> : CommonResponse
    {
        public T Data { get; set; }
    }
}
