using ProjectManager.Entity;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using ProjectManager.Student.Data;
using Radzen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Student.Pages
{
    public class IndexBase : CommonComponentBase
    {
        public StudentViewModel editModel { get; set; } = new StudentViewModel();
        public IEnumerable<Teacher> listTeacher { get; set; } = new List<Teacher>();
        public long? teacherId { get; set; }
        public string projectName { get; set; }
        public string linkDownload { get; set; }
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            Logout();

            isLoading = true;
            isShow = false;

            editModel = await _studentService.GetSelectAllByUsernameAsync(userName, token);

            var teacher = await _teacherService.GetAllTeacherAsync(token);
            listTeacher = teacher.Data.Where(x => x.DepartmentId == editModel.DepartmentId);

            var projectList = await _projectListService.GetAllProjectListAsync(token);
            var check = projectList.Data.Where(x => x.StudentId == editModel.Id);

            if (check.Count() > 0)
            {
                isShow = true;
            }

            await Delay();
            isLoading = false;
        }

        public async Task OnSubmit()
        {
            isLoading = true;
            var message = new NotificationMessage();
            message.Duration = 4000;

            var request = new ProjectList
            {
                Name = projectName,
                LinkDownload = linkDownload,
                TeacherId = teacherId,
                StudentId = editModel.Id,
                CreatedBy = userName
            };

            var result = await _projectListService.SaveAsync(request, token);

            if (result.ResponseCode == 200 && result.Data == true)
            {
                message.Severity = NotificationSeverity.Success;
                message.Summary = Constants.Message.Successfully;
            }
            else
            {
                message.Severity = NotificationSeverity.Error;
                message.Summary = Constants.Message.Fail;
            }
            message.Detail = result.ResponseMessage;
            message.Duration = 4000;
            _notificationService.Notify(message);

            await Delay();
            isLoading = false;
        }
    }
}
