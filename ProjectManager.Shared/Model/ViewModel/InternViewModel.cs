using ProjectManager.Entity;

namespace ProjectManager.Shared.Model.ViewModel
{
    public class InternViewModel : Intern
    {
        public long STT { get; set; }
        public long TotalRow { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
        public string StudentName { get; set; }
        public string ClasssName { get; set; }
        public string SpecializedName { get; set; }
        public string DepartmentName { get; set; }
        public string SchoolYearName { get; set; }
        public string StudentCode { get; set; }
    }
}
