using Microsoft.AspNetCore.Components;
using ProjectManager.Services.Admin;
using Radzen;
using System.Threading.Tasks;

namespace ProjectManager.Student.Data
{
    public abstract class CommonComponentBase : ComponentBase
    {
        [Inject] protected NavigationManager _navigationManager { get; set; }
        [Inject] protected NotificationService _notificationService { get; set; }
        [Inject] protected IStudentService _studentService { get; set; }
        [Inject] protected ITeacherService _teacherService { get; set; }
        [Inject] protected IProjectListService _projectListService { get; set; }
        [Inject] protected IAuthenticationService _authenticationService { get; set; }

        public async Task Delay()
        {
            await Task.Delay(300);
        }

        public static long projectListStatus { get; set; }

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
