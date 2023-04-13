using ProjectManager.Admin.Data;
using ProjectManager.Admin.Shared.Template;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Classs
{
    public class IndexBase : CommonComponentBase
    {
        public ClasssRequest request { get; set; } = new ClasssRequest();
        public RadzenDataGrid<ClasssViewModel> grid;
        public IEnumerable<ClasssViewModel> data;
        public IEnumerable<Entity.Department> listDepartment { get; set; }
        public IEnumerable<Entity.Specialized> listSpecialized { get; set; }
        public IEnumerable<Entity.SchoolYear> listSchoolYear { get; set; }
        public bool isLoading;
        public int count;
        public int pageSize = 10;
        public int? page = 1;

        protected override async Task OnInitializedAsync()
        {
            Logout();
            isLoading = true;

            var department = await _departmentService.GetAllDepartmentAsync(token);
            listDepartment = department.Data;

            var specialized = await _specializedService.GetAllSpecializedAsync(token);
            listSpecialized = specialized.Data;

            var schoolYear = await _schoolYearService.GetAllSchoolYearAsync(token);
            listSchoolYear = schoolYear.Data;
        }

        public async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            page = (args.Skip + pageSize) / pageSize;
            request.PageSize = pageSize;
            request.Page = page;

            var result = await _classsService.GetAllAsync(request, token);
            var message = new NotificationMessage();
            if (result.ResponseCode == 200)
            {
                data = result.Data;
                count = result.TotalRecords;
                
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
            var result = await _classsService.GetAllAsync(request, token);
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
        public async Task ShowModal(ClasssViewModel data)
        {
            await _dialogService.OpenAsync<ClasssModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
            new Dictionary<string, object>() { { "classsViewModel", data }, { "grid", grid }, { "listDepartment", listDepartment }, { "listSpecialized", listSpecialized }, { "listSchoolYear", listSchoolYear } });
        }

        public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
            new Dictionary<string, object>() { { "table", Constants.FromDelete.Classs }, { "id", id }, { "gridClass", grid } });
        }

        public async Task ShowModalViewStudent(long classsId)
        {
            await _dialogService.OpenAsync<ViewStudentModal>("Danh sách sinh viên",
            new Dictionary<string, object>() { { "classsId", classsId } },
            new DialogOptions() { Width = "700px" });
        }

    }
}
