using ProjectManager.Admin.Data;
using ProjectManager.Admin.Shared.Template;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Department
{
    public class IndexBase : CommonComponentBase
    {
        public PagingRequest request { get; set; } = new PagingRequest();
        public RadzenDataGrid<DepartmentViewModel> grid;
        public IEnumerable<DepartmentViewModel> data;
        public bool isLoading;
        public int count;
        public int pageSize = 10;
        public int? page = 1;

        protected override void OnInitialized()
        {
            Logout();
            isLoading = true;
        }

        public async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            page = (args.Skip + pageSize) / pageSize;
            request.PageSize = pageSize;
            request.Page = page;

            var result = await _departmentService.GetAllAsync(request, token);
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

        public async Task ShowModal(DepartmentViewModel data)
        {
            await _dialogService.OpenAsync<DepartmentModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
              new Dictionary<string, object>() { { "departmentViewModel", data }, { "grid", grid } });
        }

        public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
             new Dictionary<string, object>() { { "table", Constants.FromDelete.Department }, { "id", id }, { "gridDepartment", grid } });
        }

    }
}
