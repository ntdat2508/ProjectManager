﻿using ProjectManager.Entity;

namespace ProjectManager.Shared.Model.ViewModel
{
    public class StudentViewModel : Student
    {
        public long STT { get; set; }
        public long TotalRow { get; set; }
        public string SpecializedName { get; set; }
        public string TrainingSystemName { get; set; }
        public string DepartmentName { get; set; }
        public string ClasssName { get; set; }
        public string SchoolYearName { get; set; }
    }
}
