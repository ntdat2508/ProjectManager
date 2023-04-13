using ProjectManager.Entity;

namespace ProjectManager.Shared.Model.ViewModel
{
    public class SpecializedViewModel : Specialized
    {
        public long STT { get; set; }
        public long TotalRow { get; set; }
        public int Number { get; set; }
        public string DepartmentName { get; set; }

        public string CT { get; set; }
        public string TK { get; set; }
        public string UV { get; set; }
        public string GV { get; set; }
    }
}
