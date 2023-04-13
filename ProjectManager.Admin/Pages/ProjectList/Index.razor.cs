using ProjectManager.Admin.Data;
using ProjectManager.Admin.Pages.ProjectList;
using ProjectManager.Admin.Shared.Template;
using ProjectManager.Shared.Common.Enum;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.ProjectList
{
    public class IndexBase : CommonComponentBase
    {
        public ProjectListRequest request { get; set; } = new ProjectListRequest();
        public RadzenDataGrid<ProjectListViewModel> grid;
        public IEnumerable<ProjectListViewModel> data;
        public IEnumerable<Entity.Teacher> listTeacher { get; set; }
        public IEnumerable<Entity.Student> listStudent { get; set; }
        public IEnumerable<Entity.SchoolYear> listSchoolYear { get; set; }
        public List<StatusViewModel> listStatus { get; set; }
        public bool isLoading;
        public int count;
        public int pageSize = 10;
        public int? page = 1;

        protected override async Task OnInitializedAsync()
        {
            Logout();
            isLoading = true;
            request.Status = (long)StatusEnum.All;

            var teacher = await _teacherService.GetAllTeacherAsync(token);
            listTeacher = teacher.Data;

            var schoolYear = await _schoolYearService.GetAllSchoolYearAsync(token);
            listSchoolYear = schoolYear.Data;

            listStatus = _projectListService.GetAllListStatusAsync();
        }

        public async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            page = (args.Skip + pageSize) / pageSize;
            request.PageSize = pageSize;
            request.Page = page;

            if (projectListStatus > 0)
            {
                request.Status = projectListStatus;
            }

            var result = await _projectListService.GetAllAsync(request, token);
            var message = new NotificationMessage();
            if (result.ResponseCode == 200)
            {
                data = result.Data;
                count = result.TotalRecords;
                projectListStatus = -3;
            }
            else
            {
                message.Severity = NotificationSeverity.Error;
                message.Summary = Constants.Message.Fail;
                message.Detail = result.ResponseMessage;
                message.Duration = 4000;
                _notificationService.Notify(message);
            }
            await Delay();
            isLoading = false;
        }

        public async Task OnSearch()
        {
            var result = await _projectListService.GetAllAsync(request, token);
            var message = new NotificationMessage();

            if (result.ResponseCode == 200)
            {
                if (result.TotalRecords == 0)
                {
                    message.Severity = NotificationSeverity.Error;
                    message.Summary = Constants.Message.Fail;
                    message.Detail = Constants.Message.RecordNotFoundMessage;
                    message.Duration = 4000;
                    _notificationService.Notify(message);
                }
                data = result.Data; // Gán dữ liệu vào bảng
                await grid.Reload(); // Reload bảng để hiển thị dữ liệu mới
            }
        }

        public async Task ShowModal(ProjectListViewModel data)
        {
            await _dialogService.OpenAsync<ProjectListModal>("Xem dữ liệu",
            new Dictionary<string, object>() { { "projectListViewModel", data }, { "grid", grid } });
        }
        public async Task ShowModalEditProjectList(ProjectListViewModel data)
        {
            await _dialogService.OpenAsync<EditProjectListModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
            new Dictionary<string, object>() { { "ProjectListViewModel", data }, { "grid", grid }, { "listStudent", listStudent }, { "listTeacher", listTeacher } });
        }
        public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
            new Dictionary<string, object>() { { "table", Constants.FromDelete.ProjectList }, { "id", id }, { "gridProjectList", grid } });
        }

    }
}
