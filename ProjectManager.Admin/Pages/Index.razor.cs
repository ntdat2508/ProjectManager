using ProjectManager.Admin.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages
{
    public class IndexBase : CommonComponentBase
    {
        public int totalStudent;
        public int totalClass;
        public int totalDepartment;
        public int totalSpecialized;
        public int totalInternt;

        public int totalTeacher;
        public int totalProjectList;
        public int totalSuccess;
        public int totalFail;
        public bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            Logout();
            isLoading = true;

            var student = await _studentService.GetAllStudentAsync(token);
            totalStudent = student.TotalRecords;

            var classs = await _classsService.GetAllClasssAsync(token);
            totalClass = classs.TotalRecords;

            var department = await _departmentService.GetAllDepartmentAsync(token);
            totalDepartment = department.TotalRecords;

            var specialized = await _specializedService.GetAllSpecializedAsync(token);
            totalSpecialized = specialized.TotalRecords;

            var teacher = await _teacherService.GetAllTeacherAsync(token);
            totalTeacher = teacher.TotalRecords;

            //Tổng số đồ án
            var projectList = await _projectListService.GetAllProjectListAsync(token);
            totalProjectList = projectList.TotalRecords;
          

            await Delay();
            isLoading = false;

            var intern = await _internService.GetAllInternAsync(token);
            totalInternt = intern.TotalRecords;
            //totalSuccess = intern.Data.Where(x => Convert.ToDecimal(x.Point) >= Convert.ToDecimal("0")).Count();
            //totalFail = intern.Data.Where(x => Convert.ToDecimal(x.Point) < Convert.ToDecimal("0")).Count();

            await Delay();
            isLoading = false;
        }

        public void HandleClick(string url, long value)
        {
            _navigationManager.NavigateTo(url, true);
            projectListStatus = value;
        }
    }
}
