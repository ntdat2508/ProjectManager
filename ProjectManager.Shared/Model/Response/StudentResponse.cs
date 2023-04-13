using ProjectManager.Shared.Model.ViewModel;
using System.Collections.Generic;

namespace ProjectManager.Shared.Model.Response
{
    public class StudentResponse
    {
        public int TotalRecords { get; set; }
        public IEnumerable<StudentViewModel> Data { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
