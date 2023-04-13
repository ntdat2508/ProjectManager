using ProjectManager.Entity;

namespace ProjectManager.Shared.Model.ViewModel
{
    public class ClasssViewModel : Classs
    {
        public long STT { get; set; }
        public long TotalRow { get; set; }
        public string DepartmentName { get; set; }
        public string SpecializedName { get; set; }
        public string SchoolYearName { get; set; }
        public int Number { get; set; }
    }
}
