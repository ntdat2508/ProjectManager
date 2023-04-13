using Microsoft.AspNetCore.Components;
using ProjectManager.Services.Admin;
using Radzen;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Data
{
    public abstract class CommonComponentBase : ComponentBase
    {
        [Inject] protected NavigationManager _navigationManager { get; set; }
        [Inject] protected DialogService _dialogService { get; set; }
        [Inject] protected NotificationService _notificationService { get; set; }
        [Inject] protected IClasssService _classsService { get; set; }
        [Inject] protected IDepartmentService _departmentService { get; set; }
        [Inject] protected ISpecializedService _specializedService { get; set; }
        [Inject] protected ITrainingSystemService _trainingSystemService { get; set; }
        [Inject] protected ISchoolYearService _schoolYearService { get; set; }
        [Inject] protected IStudentService _studentService { get; set; }
        [Inject] protected ITeacherService _teacherService { get; set; }
        [Inject] protected IProjectListService _projectListService { get; set; }
        [Inject] protected IInternService _internService { get; set; }

        [Inject] protected IAuthenticationService _authenticationService { get; set; }

        public async Task Delay()
        {
            await Task.Delay(300);
        }

        public static long projectListStatus { get; set; }
        public static long internStatus { get; set; }

        public static string token { get; set; }
        public static string userName { get; set; }
        public static int expireTime { get; set; }

        public void Logout()
        {
            if (string.IsNullOrEmpty(token))
            {
                token = null;
                userName = null;
                expireTime = 0;
                _navigationManager.NavigateTo("/Login/Index");
            }
        }
    }
}
