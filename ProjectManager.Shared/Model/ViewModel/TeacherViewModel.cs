using ProjectManager.Entity;

namespace ProjectManager.Shared.Model.ViewModel
{
    public class TeacherViewModel : Teacher
    {
        public long STT { get; set; }
        public long TotalRow { get; set; }
        public string DepartmentName { get; set; }
        public string SpecializedName { get; set; }
    }
}
