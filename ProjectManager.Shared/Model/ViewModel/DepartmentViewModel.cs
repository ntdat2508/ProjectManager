using ProjectManager.Entity;

namespace ProjectManager.Shared.Model.ViewModel
{
    public class DepartmentViewModel : Department
    {
        public long STT { get; set; }
        public long TotalRow { get; set; }
    }
}
