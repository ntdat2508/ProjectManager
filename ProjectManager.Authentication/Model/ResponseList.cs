using System.Collections.Generic;

namespace ProjectManager.Authentication.Model
{
    public class ResponseList<T> : CommonResponse
    {
        public List<T> ListData { get; set; }
    }
}
