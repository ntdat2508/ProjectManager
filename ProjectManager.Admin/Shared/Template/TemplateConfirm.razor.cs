using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Shared.Template
{
    public class TemplateConfirmBase : CommonComponentBase
    {
        [Parameter] public string table { get; set; }
        [Parameter] public long id { get; set; }
        [Parameter] public string message { get; set; }
        [Parameter] public RadzenDataGrid<ClasssViewModel> gridClass { get; set; }
        [Parameter] public RadzenDataGrid<DepartmentViewModel> gridDepartment { get; set; }
        [Parameter] public RadzenDataGrid<SpecializedViewModel> gridSpecialized { get; set; }
        [Parameter] public RadzenDataGrid<TrainingSystemViewModel> gridTrainingSystem { get; set; }
        [Parameter] public RadzenDataGrid<SchoolYearViewModel> gridSchoolYear { get; set; }
        [Parameter] public RadzenDataGrid<TeacherViewModel> gridTeacher { get; set; }
        [Parameter] public RadzenDataGrid<StudentViewModel> gridStudent { get; set; }
        [Parameter] public RadzenDataGrid<InternViewModel> gridIntern { get; set; }

        [Parameter] public RadzenDataGrid<ProjectListViewModel> gridProjectList { get; set; }

        protected override void OnInitialized()
        {
            message = Constants.Message.ConfirmDelete;
        }

        public async Task OnSubmit()
        {
            var result = new SaveResponse();

            var deleteRequest = new DeleteRequest
            {
                Id = id,
                UserName = userName
            };

            var message = new NotificationMessage();
            message.Duration = 4000;

            switch (table)
            {
                case Constants.FromDelete.Classs:
                    result = await _classsService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.Department:
                    result = await _departmentService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.Specialized:
                    result = await _specializedService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.TrainingSystem:
                    result = await _trainingSystemService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.SchoolYear:
                    result = await _schoolYearService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.Teacher:
                    result = await _teacherService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.Student:
                    result = await _studentService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.ProjectList:
                    result = await _projectListService.DeleteAsync(deleteRequest, token);
                    break;
                case Constants.FromDelete.Intern:
                    result = await _internService.DeleteAsync(deleteRequest, token);
                    break;
                default:
                    break;
            }

            if (result.ResponseCode == 200)
            {
                Cancel();
                message.Severity = NotificationSeverity.Success;
                message.Summary = Constants.Message.Successfully;
                switch (table)
                {
                    case Constants.FromDelete.Classs:
                        await gridClass.Reload();
                        break;
                    case Constants.FromDelete.Department:
                        await gridDepartment.Reload();
                        break;
                    case Constants.FromDelete.Specialized:
                        await gridSpecialized.Reload();
                        break;
                    case Constants.FromDelete.TrainingSystem:
                        await gridTrainingSystem.Reload();
                        break;
                    case Constants.FromDelete.SchoolYear:
                        await gridSchoolYear.Reload();
                        break;
                    case Constants.FromDelete.Teacher:
                        await gridTeacher.Reload();
                        break;
                    case Constants.FromDelete.Student:
                        await gridStudent.Reload();
                        break;
                    case Constants.FromDelete.ProjectList:
                        await gridProjectList.Reload();
                        break;
                    case Constants.FromDelete.Intern:
                        await gridIntern.Reload();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                message.Severity = NotificationSeverity.Error;
                message.Summary = Constants.Message.Fail;
            }
            message.Detail = result.ResponseMessage;
            message.Duration = 4000;
            _notificationService.Notify(message);
        }

        public void Cancel()
        {
            _dialogService.Close(true);
        }
    }
}
