namespace ProjectManager.Shared.Model
{
    public class AppSettings
    {
        public string BaseUri { get; set; }

        public string Authentication { get; set; }

        //Classs (Quản lý lớp)
        public string Classs_GetAll { get; set; }
        public string Classs_Save { get; set; }
        public string Classs_Delete { get; set; }
        public string Classs_GetAllClasss { get; set; }

        //Department (Quản lý khoa)
        public string Department_GetAll { get; set; }
        public string Department_Save { get; set; }
        public string Department_Delete { get; set; }
        public string Department_GetAllDepartment { get; set; }

        //ProjectList (Danh sách đồ án)
        public string ProjectList_GetAll { get; set; }
        public string ProjectList_Save { get; set; }
        public string ProjectList_Delete { get; set; }
        public string ProjectList_Mark { get; set; }
        public string ProjectList_GetAllProjectList { get; set; }

        //ProjectList (Danh sáchthực tập)
        public string Intern_GetAll { get; set; }
        public string Intern_Save { get; set; }
        public string Intern_Delete { get; set; }
        public string Intern_Mark { get; set; }
        public string Intern_GetAllIntern { get; set; }

        //SchoolYear (Quản lý niên khóa)
        public string SchoolYear_GetAll { get; set; }
        public string SchoolYear_Save { get; set; }
        public string SchoolYear_Delete { get; set; }
        public string SchoolYear_GetAllSchoolYear { get; set; }

        //Specialized (Quản lý chuyên ngành)
        public string Specialized_GetAll { get; set; }
        public string Specialized_Save { get; set; }
        public string Specialized_Delete { get; set; }
        public string Specialized_GetAllSpecialized { get; set; }

        //Student (Quản lý sinh viên)
        public string Student_GetAll { get; set; }
        public string Student_Save { get; set; }
        public string Student_Delete { get; set; }
        public string Student_GetStudentByClasss { get; set; }
        public string Student_GetAllStudent { get; set; }
        public string Student_GetSelectAllByUsername { get; set; }

        //Teacher (Quản lý giáo viên)
        public string Teacher_GetAll { get; set; }
        public string Teacher_Save { get; set; }
        public string Teacher_Delete { get; set; }
        public string Teacher_GetTeacherBySpecialized { get; set; }
        public string Teacher_GetAllTeacher { get; set; }

        //TrainingSystem (Quản lý hệ đào tạo)
        public string TrainingSystem_GetAll { get; set; }
        public string TrainingSystem_Save { get; set; }
        public string TrainingSystem_Delete { get; set; }
        public string TrainingSystem_GetAllTrainingSystem { get; set; }
    }
}
