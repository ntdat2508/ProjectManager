using ProjectManager.Shared.Model.ViewModel;
using System.Collections.Generic;

namespace ProjectManager.Shared.Model.Response
{
    public class ClasssResponse
    {
        public int TotalRecords { get; set; }
        public IEnumerable<ClasssViewModel> Data { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
