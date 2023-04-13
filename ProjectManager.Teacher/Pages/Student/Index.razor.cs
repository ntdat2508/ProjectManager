using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using ProjectManager.Teacher.Data;
using ProjectManager.Teacher.Shared.Template;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Teacher.Pages.Student
{
    public class IndexBase : CommonComponentBase
    {
        public StudentRequest request { get; set; } = new StudentRequest();
        public RadzenDataGrid<StudentViewModel> grid;
        public IEnumerable<StudentViewModel> data;
        public IEnumerable<Entity.Department> listDepartment { get; set; }
        public IEnumerable<Entity.Specialized> listSpecialized { get; set; }
        public IEnumerable<Entity.Classs> listClasss { get; set; }
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

            var classs = await _classsService.GetAllClasssAsync(token);
            listClasss = classs.Data;
        }

        public async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            page = (args.Skip + pageSize) / pageSize;
            request.PageSize = pageSize;
            request.Page = page;

            var result = await _studentService.GetAllAsync(request, token);
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
            await grid.FirstPage();
        }

        public async Task ShowModal(StudentViewModel data)
        {
            await _dialogService.OpenAsync<StudentModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
            new Dictionary<string, object>() { { "studentViewModel", data }, { "grid", grid }, { "listDepartment", listDepartment }, { "listSpecialized", listSpecialized }, { "listClasss", listClasss } });
        }

        public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
            new Dictionary<string, object>() { { "table", Constants.FromDelete.Student }, { "id", id }, { "gridStudent", grid } });
        }

    }
}
