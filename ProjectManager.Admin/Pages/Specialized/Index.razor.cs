using ProjectManager.Admin.Data;
using ProjectManager.Admin.Shared.Template;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Specialized
{
    public class IndexBase : CommonComponentBase
    {
        public PagingRequest request { get; set; } = new PagingRequest();
        public RadzenDataGrid<SpecializedViewModel> grid;
        public IEnumerable<SpecializedViewModel> data;
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

            var result = await _teacherService.GetTeacherBySpecializedAsync(request, token);
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
            var result = await _specializedService.GetAllAsync(request, token);
            if (result.ResponseCode == 200)
            {
                data = result.Data; // Gán dữ liệu vào bảng
                await grid.Reload(); // Reload bảng để hiển thị dữ liệu mới
            }
        }

        public async Task ShowModal(SpecializedViewModel data)
        {
            await _dialogService.OpenAsync<SpecializedModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
              new Dictionary<string, object>() { { "specializedViewModel", data }, { "grid", grid } });
        }

        public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
             new Dictionary<string, object>() { { "table", Constants.FromDelete.Specialized }, { "id", id }, { "gridSpecialized", grid } });
        }
        public async Task ShowModalViewTeacher(long specializedId)
        {
            await _dialogService.OpenAsync<ViewTeacherModal>("Danh sách giảng viên",
            new Dictionary<string, object>() { { "specializedId", specializedId } },
            new DialogOptions() { Width = "700px" });
        }
    }
}
