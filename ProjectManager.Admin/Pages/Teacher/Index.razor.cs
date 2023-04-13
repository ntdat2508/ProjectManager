using ProjectManager.Admin.Data;
using ProjectManager.Admin.Shared.Template;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Teacher
{
    public class IndexBase : CommonComponentBase
    {
        public TeacherRequest request { get; set; } = new TeacherRequest();
        public RadzenDataGrid<TeacherViewModel> grid;
        public IEnumerable<TeacherViewModel> data;
        public IEnumerable<Entity.Department> listDepartment { get; set; }
        public IEnumerable<Entity.Specialized> listSpecialized { get; set; }
        public bool isLoading;
        public int count;
        public int pageSize = 10;
        public int? page = 1;
        public class genderoj
        {
            public string gender { get; set; }

            public genderoj(string g)
            {
                this.gender = g;
            }

        }
        List<genderoj> listGender = new List<genderoj>();
        protected override async Task OnInitializedAsync()
        {
            Logout();
            isLoading = true;
            var department = await _departmentService.GetAllDepartmentAsync(token);
            listDepartment = department.Data;
            listGender.Add(new genderoj("Nam"));
            listGender.Add(new genderoj("Nữ"));
            var specialized = await _specializedService.GetAllSpecializedAsync(token);
            listSpecialized = specialized.Data;
        }

        public async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            page = (args.Skip + pageSize) / pageSize;
            request.PageSize = pageSize;
            request.Page = page;

            var result = await _teacherService.GetAllAsync(request, token);
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
            var result = await _teacherService.GetAllAsync(request, token);
            if (result.ResponseCode == 200)
            {
                data = result.Data; // Gán dữ liệu vào bảng
                await grid.Reload(); // Reload bảng để hiển thị dữ liệu mới
            }
        }

        public async Task ShowModal(TeacherViewModel data)
        {
            await _dialogService.OpenAsync<TeacherModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
            new Dictionary<string, object>() { { "teacherViewModel", data }, { "grid", grid }, { "listDepartment", listDepartment }, { "listSpecialized", listSpecialized }, { "listGender", listGender } });
        }

        public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
            new Dictionary<string, object>() { { "table", Constants.FromDelete.Teacher }, { "id", id }, { "gridTeacher", grid } });
        }

    }
}
